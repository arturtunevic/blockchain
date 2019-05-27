using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain
{
    class DAO
    {
        List<Candidate> kandidatai;

        public DAO()
        {
            kandidatai = new List<Candidate>();
        }

        public List<Candidate> CreateCandidates()
        {
            Candidate kandidatas1 = new Candidate("Ingrida Simonyte", "simonyte.jpg");
            Candidate kandidatas2 = new Candidate("Gitanas Nauseda", "nauseda.jpg");
            Candidate kandidatas3 = new Candidate("Saulius Skvernelis", "skvernelis.jpg");
            kandidatai.Add(kandidatas1);
            kandidatai.Add(kandidatas2);
            kandidatai.Add(kandidatas3);
            return kandidatai;
        }
    }
}
