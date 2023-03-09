using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Models.Trees;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;

namespace Our.Umbraco.Synthscribe.NotificationHandlers
{
    internal class MenuRenderingNotificationHandler : INotificationHandler<MenuRenderingNotification>
    {
        private readonly ILocalizationService _localizationService;

        public MenuRenderingNotificationHandler(ILocalizationService localizationService)
        {
            _localizationService = localizationService;

        }

        public void Handle(MenuRenderingNotification notification)
        {

            var menuText = $"Translate";

            if (notification.TreeAlias.Equals(Constants.Trees.Dictionary))
            {
                var dictionary = _localizationService.GetDictionaryItemById(Convert.ToInt32(notification.NodeId));
                var title = "all dictionaries";
                //Is root tree item
                if (notification.NodeId == "-1")
                    menuText = "Translate all dictionaries";
                else
                    title = dictionary.ItemKey;

                var menuItem = new MenuItem("translate", menuText);
                menuItem.AdditionalData.Add("actionView", $"/App_Plugins/Our.Umbraco.Synthscribe/backoffice/translation/dictionary/edit.html");
                menuItem.Icon = "umb-translation";
                menuItem.SeparatorBefore = true;

                //Add dictionary id
                menuItem.AdditionalData.Add("nodeId", notification.NodeId);

                //Add dictionary key
                menuItem.AdditionalData.Add("dictionaryKey", title);

                notification.Menu.Items.Add(menuItem);
            }
            if (notification.TreeAlias.Equals(Constants.Trees.Content))
            {
                var menuItem = new MenuItem("translate", menuText);
                menuItem.AdditionalData.Add("actionView", "/App_Plugins/Our.Umbraco.Synthscribe/backoffice/translation/content/edit.html");
                menuItem.Icon = "umb-translation";
                menuItem.AdditionalData.Add("nodeId", notification.NodeId);
                notification.Menu.Items.Add(menuItem);
            }
        }
    }
}
