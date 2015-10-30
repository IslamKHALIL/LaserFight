using LaserFight.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LaserFight.ResultsDashboard.ViewModels
{
    public class Player : BindableBase
    {
        public string Id { get; set; }
        
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProperty<string>(ref _Name, value); }
        }

        private int _Score;
        public int Score
        {
            get { return _Score; }
            set { SetProperty<int>(ref _Score, value); }
        }

        private string _Background;
        public string Background
        {
            get { return _Background; }
            set { SetProperty<string>(ref _Background, value); }
        }
    }
}
