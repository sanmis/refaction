using AutoMapper;

namespace refactor_me.Logic
{
    public class AbstractLogic
    {
        public AbstractLogic(IMapper mapper)
        {
            Mapper = mapper;

        }

        public IMapper Mapper { get; set; }
    }
}
