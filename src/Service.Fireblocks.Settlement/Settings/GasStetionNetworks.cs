using System.Collections.Generic;

namespace Service.Fireblocks.Settlement.Settings
{
    public static class GasStetionNetworks
    {
        private static HashSet<string> _gasStationNetworksCache = null;
        public static HashSet<string> GasStationNetworksCache
        {
            get
            {
                if (_gasStationNetworksCache == null)
                {
                    _gasStationNetworksCache = new HashSet<string>();

                    var networks = Program.Settings.GasStationNetworks.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in networks)
                    {
                        _gasStationNetworksCache.Add(item);
                    }
                }

                return _gasStationNetworksCache;
            }
        }


    }
}
