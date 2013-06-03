namespace WindowsPhoneTestFramework.Client.AutomationClient.Remote
{
    using System.Linq;
    using System.Windows.Automation;
    using Microsoft.Phone.Controls;
    using WindowsPhoneTestFramework.Client.AutomationClient.Helpers;

    public partial class PivotCommand : AutomationElementCommandBase
    {
        protected override void DoImpl()
        {
            var element = GetFrameworkElement(false);
            if (element == null)
            {
                SendNotFoundResult(string.Format("PivotCommand: Could not find the element : {0}", AutomationIdentifier.ToIdOrName()));
                return;
            }

            var pivot = element as Pivot;

            if (pivot != null)
            {
                if (MovePivot(pivot))
                {
                    SendSuccessResult();
                }
            }
            else
            {
                var pano = element as Panorama;

                if (pano != null)
                {
                    if (MovePano(pano))
                    {
                        SendSuccessResult();
                    }
                }
                else
                {
                    SendNotFoundResult("PivotCommand: Could not find the Panorama");
                }
            }
        }

        private bool MovePivot(Pivot pivot)
        {
            bool result = false;
            if (PivotNext)
            {
                pivot.SelectedIndex = (pivot.SelectedIndex + 1) % pivot.Items.Count;
                result = true;
            }
            else if (PivotLast)
            {
                if (pivot.SelectedIndex == 0)
                {
                    pivot.SelectedIndex = pivot.Items.Count - 1;
                    result = true;
                }
                else
                {
                    pivot.SelectedIndex = pivot.SelectedIndex - 1;
                    result = true;
                }
            }
            else if (!string.IsNullOrWhiteSpace(PivotName))
            {
                var index = 0;
                var pivotFound = false;

                foreach (var pivotItem in pivot.Items.Cast<PivotItem>())
                {
                    if (pivotItem.Name.ToLowerInvariant().StartsWith(PivotName.ToLowerInvariant()) ||
                        pivotItem.GetValue(AutomationProperties.NameProperty).ToString().ToLowerInvariant().StartsWith(PivotName.ToLowerInvariant()))
                    {
                        pivotFound = true;
                        break;
                    }
                    index++;
                }

                if (pivotFound)
                {
                    pivot.SelectedIndex = index;
                    result = true;
                }
                else
                {
                    SendNotFoundResult(string.Format("PivotCommand: Could not find the Pivot : {0}", PivotName));
                }
            }
            else
            {
                SendNotFoundResult("PivotCommand: Could not find the pivot");
            }

            return result;
        }

        private bool MovePano(Panorama pano)
        {
            bool result = false;
            if (PivotNext)
            {
                pano.DefaultItem = pano.Items[(pano.SelectedIndex + 1) % pano.Items.Count];
                result = true;
            }
            else if (PivotLast)
            {
                if (pano.SelectedIndex == 0)
                {
                    pano.DefaultItem = pano.Items[pano.Items.Count - 1];
                    result = true;
                }
                else
                {
                    pano.DefaultItem = pano.Items[pano.SelectedIndex - 1];
                    result = true;
                }
            }
            else if (!string.IsNullOrWhiteSpace(PivotName))
            {
                var panoToGo = pano.Items.Cast<PanoramaItem>().FirstOrDefault(item => item.Name.ToLowerInvariant().StartsWith(PivotName.ToLowerInvariant())
                                                                || item.GetValue(AutomationProperties.NameProperty).ToString().StartsWith(PivotName.ToLowerInvariant()));

                if (panoToGo != null)
                {
                    pano.DefaultItem = panoToGo;
                    result = true;
                }
                else
                {
                    SendNotFoundResult(string.Format("PivotCommand: Could not find the Panorama : {0}", PivotName));
                }
            }
            else
            {
                SendNotFoundResult("PivotCommand: Could nto find the Panorama element");
            }

            return result;
        }
    }
}