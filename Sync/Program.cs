using AutoMapper;
using Sync.DTO;
using Sync.QueryBuilders;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sync
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Sync().GetAwaiter().GetResult();
        }

        private static async Task Sync()
        {
            var apiKey = "v_eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiN2VhYTBhNTQtYTBiZC00OTNlLWFjNDMtZjNjZGEwZmVlNWQ5IiwiZXhwIjoyMTQ3NDgzNjQ3LCJpc3MiOiJodHRwczovL2FwcC52aXJ0dW91c3NvZnR3YXJlLmNvbSIsImF1ZCI6Imh0dHBzOi8vYXBpLnZpcnR1b3Vzc29mdHdhcmUuY29tIn0.oN0bfmYMS7lPxGtVH3ouEVhD0Kuzoqa2nAnuvPTyPpk";
            var configuration = new Configuration(apiKey);
            var virtuousService = new VirtuousService(configuration);

            var skip = 0;
            var take = 100;
            var maxContacts = 1000;

            using (var context = new VirtuousDbContext())
            {
                var queryGroups = new GroupBuilder()
                    .AddGroup(new IsStateCondition("AZ")) // add other conditions like contains "Contact Name" new ContainsContactNameCondition("Rony"))
                    .AddGroup(new IsStateCondition("FL")) // customize with different query groups
                    .Build();

                // configure automapper
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<AbbreviatedContactDTO, AbbreviatedContact>()
                       .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
                });

                var mapper = config.CreateMapper(); // TODO: have these depedencies injected

                while (skip < maxContacts)
                {
                    var contacts = await virtuousService.GetContactsAsync(skip, take, queryGroups);

                    if (contacts.List == null || !contacts.List.Any()) break;

                    // we want to check if the contacts already exists to avoid adding duplicates
                    var payloadIds = contacts.List.Select(c => c.Id).ToList(); 

                    var existingIds = context.Contacts
                                              .Where(c => payloadIds.Contains(c.ExternalId))
                                              .Select(c => c.ExternalId)
                                              .ToHashSet(); //hashset enhances lookup efficiency with Contains()

                    var newContacts = contacts.List.Where(dto => !existingIds.Contains(dto.Id)).ToList();

                    var contactsMapped = mapper.Map<List<AbbreviatedContact>>(newContacts);

                    // add only the new contacts
                    if (contactsMapped.Any())
                    {
                        context.Contacts.AddRange(contactsMapped);
                        await context.SaveChangesAsync();
                    }
                    skip += take;
                }
            }
        }
    }
}
