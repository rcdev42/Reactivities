using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _dbcontext;

            public Handler(DataContext dbcontext)
            {
                _dbcontext = dbcontext;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _dbcontext.Activities.FindAsync(request.Id);

                if (activity == null)
                    throw new Exception("Could not find activity");

                _dbcontext.Remove(activity);

                bool success = await _dbcontext.SaveChangesAsync() > 0;

                if (success)
                    return Unit.Value;
                else
                    throw new Exception("Problem saving changes");
            }
        }

    }
}