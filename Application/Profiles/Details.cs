using Application.Core;
using MediatR;
using Application.Profiles;
using Persistence;
using AutoMapper;
using System.Threading.Tasks;
using System.Threading;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Profiles
{
    public class Details
    {
        public class Query : IRequest<Result<Application.Profiles.Porfile>>
        {
            public string UserName {get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Porfile>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<Application.Profiles.Porfile>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.ProjectTo<Application.Profiles.Porfile>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.UserName == request.UserName);

                if(user == null)
                {
                    return null;
                }

                return Result<Application.Profiles.Porfile>.Success(user);
            }
        }
    }
}