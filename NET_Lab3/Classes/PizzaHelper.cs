using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NET_Lab3.Classes
{
    static class PizzaHelper
    {
        public static Dictionary<string, string> ToppingsCartNames = new Dictionary<string, string>
        {
            {"Ser", "serem"},
            {"Pieczarki", "pieczarkami"},
            {"Pepperoni", "pepperoni"},
            {"Szynka", "szynką"},
            {"Ananas", "ananasem"},
            {"Brokuły", "brokułami"},
            {"Kukurydza", "kukurydzą"},
            {"Cebula", "cebulą"},
            {"Kurczak", "kurczakiem"},
            {"Rukola", "rukolą"}
        };

        public static Dictionary<string, double> ToppingsCosts = new Dictionary<string, double>
        {
            {"Ser", 4.00},
            {"Pieczarki", 3.00},
            {"Pepperoni", 5.00},
            {"Szynka", 4.00},
            {"Ananas", 3.50},
            {"Brokuły", 3.00},
            {"Kukurydza", 3.00},
            {"Cebula", 2.50},
            {"Kurczak", 4.50},
            {"Rukola", 2.50}
        };

        public static Dictionary<string, double> PizzaCosts = new Dictionary<string, double>
        {
            {"Margherita", 20.00},
            {"Hawajska", 23.50},
            {"Prosciutto", 26.00},
            {"Capricciosa", 27.00},
            {"Pepperoni", 27.00},
            {"Vegetariana", 26.50}
        };

        public static Dictionary<double, string> SauceCartNames = new Dictionary<double, string>
        {
            {0, "łagodnym"},
            {1, "pikantnym"},
            {2, "ostrym"},
            {3, "mega ostrym"}
        };

        public static Dictionary<string, double> SizeCostMultipliers = new Dictionary<string, double>
        {
            {"Mała", 1.00},
            {"Duża", 1.40},
            {"Mega", 1.80}
        };

        public static string ToLowerFirstChar(string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;
            return s[0].ToString().ToLower() + s.Substring(1);
        }


        public static string GetSummaryPizzaString(string selectedPizza, string selectedSize, double selectedSauce, List<string> selectedToppings)
        {
            return selectedSize.Substring(0, 4) + " " + ToLowerFirstChar(selectedPizza) + " z " + (selectedToppings.Count > 0 ? GetSummaryToppingsString(selectedToppings) : "")
                              + "sosem " + SauceCartNames[selectedSauce];
        }

        public static string GetSummaryToppingsString(List<string> selectedToppings)
        {
            string summaryToppingsString = "";
            foreach (string topping in selectedToppings)
            {
                summaryToppingsString += ToppingsCartNames[topping.Split(' ')[0]] + ", ";
            }
            return summaryToppingsString.Substring(0, summaryToppingsString.Length - 2) + " i ";
        }

        public static double GetSummaryPizzaCost(string selectedPizza, string selectedSize, List<string> selectedToppings)
        {
            return (PizzaCosts[selectedPizza] * SizeCostMultipliers[selectedSize.Substring(0, 4)]) + (selectedToppings.Count > 0 ? GetSummaryToppingsCost(selectedToppings) : 0.00);
        }

        public static double GetSummaryToppingsCost(List<string> selectedToppings)
        {
            double summaryToppingsCost = 0.00;
            foreach (string topping in selectedToppings)
            {
                summaryToppingsCost += ToppingsCosts[topping.Split(' ')[0]];
            }
            return summaryToppingsCost;
        }
    }
}
