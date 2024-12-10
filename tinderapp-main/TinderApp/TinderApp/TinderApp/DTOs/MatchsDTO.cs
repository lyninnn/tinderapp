using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TinderApp.DTOs
{
    public partial class MatchsDTO: ObservableObject
    {

        [ObservableProperty]
        private int matchId;

        [ObservableProperty]
        private int usuario1Id;

        [ObservableProperty]
        private int usuario2Id;

        [ObservableProperty]
        private DateTime fechaMatch;

    }
}
