using System;
namespace Serv4Fish3.Model
{
    public class Result
    {
        public int Id;
        public int UserId;
        public int Totalresult;
        public int Winresult;

        public Result(int id, int userid, int totalCount, int winCount)
        {
            this.Id = id;
            this.UserId = userid;
            this.Totalresult = totalCount;
            this.Winresult = winCount;
        }



    }
}
