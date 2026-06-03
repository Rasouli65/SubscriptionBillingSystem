using MediatR;
using Microsoft.EntityFrameworkCore;
using SubscriptionBillingSystem.Application.Abstractions;
using SubscriptionBillingSystem.Domain.Aggregates.Customer;
using SubscriptionBillingSystem.Domain.Aggregates.Invoice;
using SubscriptionBillingSystem.Domain.Aggregates.Subscription;
using SubscriptionBillingSystem.Domain.Common;

namespace SubscriptionBillingSystem.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator)
           : base(options)
        {
            _mediator = mediator;
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Invoice> Invoices { get; set; }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.Entity<Invoice>(builder =>
            {
                // Commented out because EF InMemory provider does not fully support this mapping.
                //builder.ComplexProperty(s => s.Amount, priceBuilder =>
                //{
                //    priceBuilder.Property(p => p.Amount);
                //    priceBuilder.Property(p => p.Currency);
                //});

                builder.OwnsOne(s => s.Amount, priceBuilder =>
                {
                    priceBuilder.Property(p => p.Amount);
                    priceBuilder.Property(p => p.Currency);
                });
            });

            modelBuilder.Entity<Subscription>(builder =>
            {
                // Commented out because EF InMemory provider does not fully support this mapping.
                //builder.ComplexProperty(s => s.Price, priceBuilder =>
                //{
                //    priceBuilder.Property(p => p.Amount);
                //    priceBuilder.Property(p => p.Currency);
                //});

                builder.OwnsOne(s => s.Price, priceBuilder =>
                {
                    priceBuilder.Property(p => p.Amount);
                    priceBuilder.Property(p => p.Currency);
                });
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEntities = ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            var result = await base.SaveChangesAsync(cancellationToken);

            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }

            return result;
        }
    } 
}
