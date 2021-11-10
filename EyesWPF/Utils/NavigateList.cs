using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyesWPF.Utils
{
    class NavigateList
    {
        #region Свойства страницы

        public int NumberPage { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int CountOutAgents { get { return 10; } }

        #endregion

        #region Свойства, предназначеные для условий 

        public int TotalPage
        {
            get
            {
                return (int)Math.Ceiling((decimal)EndIndex / CountOutAgents);
            }
        }

        public bool UsedBySearch
        {
            get
            {
                return EndIndex <= CountOutAgents;
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return NumberPage > 1;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return NumberPage <= TotalPage;
            }
        }

        #endregion

        #region Конструктор

        public NavigateList()
        {
            NumberPage = 1;
            StartIndex = 0;
        }

        #endregion

        #region Метод нахождения открывшейся страницы

        public void GetIndex()
        {
            StartIndex = 0;

            if (HasPreviousPage && HasNextPage)
            {
                for (int i = 1; i < NumberPage; i++)
                    StartIndex += CountOutAgents;
            }
        }

        #endregion
    }
}
