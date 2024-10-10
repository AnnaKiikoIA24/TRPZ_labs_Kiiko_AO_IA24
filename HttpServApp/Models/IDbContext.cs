using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServApp.Models
{
    interface IDbContext
    {
        public void AddRequest(HttpRequest request);

        public void RemoveRequest(HttpRequest request);

        public void UpdateRequest(HttpRequest request);

        public HttpRequest? GetRequestById(long idRequest);

        public List<HttpRequest> GetRequestsByPeriod(DateTime dateBeg, DateTime dateEnd);

        public void LoadFromDb();

        public void SaveToDB(HttpRequest httpRequest, char typeOper);
    }
}
