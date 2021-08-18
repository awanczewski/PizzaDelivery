using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NET_Lab3.Classes
{
    static class DeliveryHelper
    {

        public static string GetDeliveryAddressString(string street, string buildingNumber, string apartmentNumber)
        {
            return street + " " + buildingNumber + (string.IsNullOrEmpty(apartmentNumber) ? "" : "/" + apartmentNumber);
        }

        public static bool IsAddressValid(string street, string buildingNumber, string apartmentNumber)
        {
            if (string.IsNullOrEmpty(street) || string.IsNullOrEmpty(buildingNumber))
            {
                return false;
            }
            else
            {
                if (!new Regex(@"^\d+[a-zA-Z]?$").IsMatch(buildingNumber))
                {
                    return false;
                }
                if(!new Regex(@"\w*[a-zA-Z]\w*").IsMatch(street))
                {
                    return false;
                }
                else if(!string.IsNullOrEmpty(apartmentNumber))
                {
                    if (new Regex(@"^\d+[a-zA-Z]?$").IsMatch(apartmentNumber))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
        }

        public static bool IsPhoneNumberValid(string phoneNumber)
        {

            if (string.IsNullOrEmpty(phoneNumber))
            {
                return false;
            }
            else
            {
                if (new Regex(@"^\d{9}$").IsMatch(phoneNumber))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }


        public static bool IsPizzeriaOpen()
        {
            if(DateTime.Now.Hour < 23 && DateTime.Now.Hour >= 11)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsDeliveryTimeValid(string selectedDeliveryTime)
        {
            if(string.IsNullOrEmpty(selectedDeliveryTime))
            {
                return false;
            }
            else
            {
                if(selectedDeliveryTime == "Tak szybko jak to możliwe")
                {
                    return true;
                }
                else
                {
                    if (DateTime.Now.AddMinutes(30) < DateTime.Parse(selectedDeliveryTime.Substring(0, 5)))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }


        public static List<string> GetDeliveryTimeList()
        {
            List<string> deliveryTimeList = new List<string>();

            DateTime firstDateTime;
            DateTime secondDateTime;

            firstDateTime = DateTime.Now.AddMinutes(45);

            while (firstDateTime.Minute % 30 != 0)
            {
                firstDateTime = firstDateTime.AddMinutes(1);
            }

            while (firstDateTime.Hour < 23 && firstDateTime.Hour > 11)
            {
                secondDateTime = firstDateTime.AddMinutes(30);

                deliveryTimeList.Add(firstDateTime.ToString("HH:mm") + " - " + secondDateTime.ToString("HH:mm"));

                firstDateTime = secondDateTime;
            }

            return deliveryTimeList;
        }
    }
}
