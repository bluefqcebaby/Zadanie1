namespace Zadanie1
{
    class SortModel
    {
        public SortModel(int id, string name)
        {

            this.id = id;
            this.name = name;
        }

        public int id { get; set; }

        public string name { get; set; }

    }

    class FilterModel
    {
        public int ID_Type_agent { get; set; }
        public string Name { get; set; }

        public FilterModel(int id_type_agent, string name)
        {
            ID_Type_agent = id_type_agent;
            Name = name;
        }
    }

    public class AgentsView
    {
        public int Id { get; set; }
        public string TypeAndName { get; set; }
        public string SaleCount { get; set; }
        public int SaleNumber { get; set; }
        public string Logo { get; set; }
        public string Phone { get; set; }
        public string Priority { get; set; }
        public int PriorityId { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }

        public AgentsView(int id, string typeAndName, string saleCount, string logo, int saleNumber, string phone, string priority, int priorityId, string name, int typeId)
        {
            Id = id;
            TypeAndName = typeAndName;
            SaleCount = saleCount;
            Logo = logo;
            SaleNumber = saleNumber;
            Phone = phone;
            Priority = priority;
            PriorityId = priorityId;
            Name = name;
            TypeId = typeId;
        }
    }

    public partial class Agents
    {
        public Agents(int iD_Agent, string name, string email, string phone, string logo, string adress, int priority, string director, string iNN,
            string kPP, int iD_Type_Agents)
        {
            ID_Agent = iD_Agent;
            this.name = name;
            this.email = email;
            this.phone = phone;
            this.logo = logo;
            this.adress = adress;
            this.priority = priority;
            this.director = director;
            INN = iNN;
            KPP = kPP;
            ID_Type_Agents = iD_Type_Agents;
        }
    }
}
