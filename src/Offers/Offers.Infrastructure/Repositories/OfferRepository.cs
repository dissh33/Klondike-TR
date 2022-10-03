using System.Data;
using App.Metrics;
using Offers.Application.Contracts;
using Offers.Domain.Entities;
using Serilog;

namespace Offers.Infrastructure.Repositories;

public class OfferRepository : BaseRepository<Offer>, IOfferRepository
{
    public OfferRepository(IDbTransaction transaction, ILogger logger, IMetrics metrics) 
        : base(transaction, logger, metrics)
    {
    }
    


}