using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotNetLearning.EmployeeManager
{
    public class DataStore
    {
        private static IList<Country> _countryList = null;
        private static IList<State> _stateList = null;
        private static IList<Plant> _plantList = null;
        private static IList<Employee> _employeeList = null;

        public static IList<Country> GetCountryList()
        {
            if (_countryList == null)
            {
                _countryList = new List<Country>()
                    {
                        new Country(){ CountryId=0, CountryName="--Select--"},
                        new Country(){ CountryId=1, CountryName="India"},
                        new Country(){ CountryId=2, CountryName="USA"},

                   };
            }
            return _countryList;
        }
        public static IList<State> GetStateByCountryId(int CountryId)
        {
            if (_stateList == null)
            {
                _stateList = new List<State>()
                 {
                        new State(){ CountryId=0, StateId=0 , StateName="--Select--"},
                        new State(){ CountryId=1, StateId=1 , StateName="UP"},
                        new State(){ CountryId=1,  StateId=2 , StateName="Gujrat"},
                        new State(){ CountryId=2, StateId=3 , StateName="Newyork"},
                        new State(){ CountryId=2,  StateId=4 , StateName="Texas"},

                   };
            }
            return _stateList.Where(state => state.CountryId == CountryId).ToList();
        }

        public static Plant GetPlantByPlantId(int plantId)
        {
            if (_plantList != null)
            {
                return _plantList.FirstOrDefault(plant => plant.PlantId == plantId);
            }
            else
                return null;
        }

        public static Country GetCountryByCountryId(int countryId)
        {
            if (_countryList != null)
            {
                return _countryList.FirstOrDefault(country => country.CountryId == countryId);
            }
            else
                return null;
        }
        public static State GetStateByStateId(int stateId)
        {
            if (_stateList != null)
            {
                return _stateList.FirstOrDefault(state => state.StateId == stateId);
            }
            else
                return null;
        }
        public static List<Plant> GetPlantByStateId(int StateId)
        {
            if (_plantList == null)
            {
                _plantList = new List<Plant>()
                 {
                        new Plant(){ PlantId=0,  StateId=0 ,  PlantName="--Select--"},
                        new Plant(){ PlantId=1,  StateId=1 ,  PlantName="Noida-Plant"},
                        new Plant(){ PlantId=2,  StateId=1 ,  PlantName="Meerut-Plant"},
                        new Plant(){ PlantId=3,  StateId=2 ,  PlantName="Surat-Plant"},
                        new Plant(){ PlantId=4,  StateId=2 ,  PlantName="Vadodra-Plant"},
                        new Plant(){ PlantId=5,  StateId=3 ,  PlantName="Amsterdem-Plant"},
                        new Plant(){ PlantId=6,  StateId=3 ,  PlantName="berlin-Plant"},
                        new Plant(){ PlantId=7,  StateId=4 ,  PlantName="Dallas-Plant"},
                        new Plant(){ PlantId=8,  StateId=4 ,  PlantName="Austin-Plant" }

                   };
            }
            return _plantList.Where(plant => plant.StateId == StateId).ToList();
        }
        public static List<Employee> GetEmployeesByPlantId(int PlantId)
        {
            if (_employeeList == null)
            {
                _employeeList = new List<Employee>()
                 {

                        new Employee(){ ID=1,   PlantId=1 ,  Name="A", Age=22, salary= 20000},
                        new Employee(){ ID=2,   PlantId=1 ,  Name="B", Age=22, salary= 20000},
                        new Employee(){ ID=3,   PlantId=2 ,  Name="C", Age=22, salary= 20000},
                        new Employee(){ ID=4,   PlantId=2 ,  Name="D", Age=22, salary= 20000},
                        new Employee(){ ID=5,   PlantId=3 ,  Name="E", Age=22, salary= 20000},
                        new Employee(){ ID=6,   PlantId=3 ,  Name="F", Age=22, salary= 20000},
                        new Employee(){ ID=7,   PlantId=4 ,  Name="G", Age=22, salary= 20000},
                        new Employee(){ ID=8,   PlantId=4 ,  Name="H", Age=22, salary= 20000},
                        new Employee(){ ID=9,   PlantId=5 ,  Name="I", Age=22, salary= 20000},
                        new Employee(){ ID=10,  PlantId=5 ,  Name="J", Age=22, salary= 20000},
                        new Employee(){ ID=11,  PlantId=6 ,  Name="K", Age=22, salary= 20000},
                        new Employee(){ ID=12,  PlantId=6 ,  Name="L", Age=22, salary= 20000},
                        new Employee(){ ID=13,  PlantId=7 ,  Name="M", Age=22, salary= 20000},
                        new Employee(){ ID=14,  PlantId=7 ,  Name="N", Age=22, salary= 20000},
                        new Employee(){ ID=15,  PlantId=8 ,  Name="O", Age=22, salary= 20000},
                        new Employee(){ ID=16,  PlantId=8 ,  Name="P", Age=22, salary= 20000}

                   };
            }
            return _employeeList.Where(employee => employee.PlantId == PlantId).ToList();
        }

        public static Employee CreateEmployee(int plantId)
        {
            List<Employee> employeeList = GetEmployeesByPlantId(plantId);

            return new Employee()
            {
                ID = employeeList.Count > 0 ? employeeList.Max(emp => emp.ID) + 1 : 1
            };
        }

        public static bool AddEmployee(Employee employee)
        {
            bool success = false;
            if (employee != null)
            {
                List<Employee> employeeList = GetEmployeesByPlantId(employee.PlantId);
                if (!employeeList.Any(emp=> emp.ID== employee.ID))
                {
                    _employeeList.Add(employee);
                    success = true;
                }                
            }
            return success;
        }

        public static bool RemoveEmployee(Employee employee)
        {
            bool success = false;
            if (employee != null)
            {
                List<Employee> employeeList = GetEmployeesByPlantId(employee.PlantId);
                if (employeeList.Any(emp => emp.ID == employee.ID))
                {
                    _employeeList.Remove(employee);
                }
                     
            }
            return success;
        }
      
    }
}
