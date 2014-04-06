using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace mgb.derby
{    
    public partial class Configuration : Window
    {
        private Boolean _alreadySaved;
        private Race _savedRace;
        private Timer _savedTimer; // **********todo: Add code for this

        public Configuration()
        {
            InitializeComponent();
            _savedRace = new Race();
        }

        private void Config_Loaded(object sender, RoutedEventArgs e)
        {               
            btnEdit.IsEnabled = false;
            btnSaveRaceSettings.IsEnabled = true;
            _alreadySaved = false;

            tbDescription.Focus();
        }

        private void btnSaveRaceSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new DerbyDBEntities())
                {
                    context.Database.Connection.Open();

                    // Create the race
                    Race theRace = null;

                    if (_alreadySaved == false)
                    {
                        theRace = new Race();
                    }
                    else
                    {
                        theRace = _savedRace;
                    }

                    theRace.Description = tbDescription.Text;
                    theRace.Date = Convert.ToDateTime(tbDate.Text);
                    theRace.ScoringMethod = cbScoringMethod.Text;
                    theRace.Rounds = Convert.ToInt32(cbRounds.Text);
                    theRace.Lanes = Convert.ToInt32(cbLanes.Text);

                    context.Races.Add(theRace);                    
                    context.SaveChanges();

                    _savedRace.Description = theRace.Description;
                    _savedRace.Date = theRace.Date;
                    _savedRace.ScoringMethod = theRace.ScoringMethod;
                    _savedRace.Rounds = theRace.Rounds;
                    _savedRace.Lanes = theRace.Lanes;

                    // Create the Timer
                    Timer newTimer = new Timer();
                    newTimer.RaceId = theRace.RaceID;
                    newTimer.Description = cbTimer.Text; 
                    newTimer.SerialPort = cbSerialPort.Text;
                    newTimer.BaudRate = cbBaudRate.Text;
                    newTimer.DataBits = cbDataBits.Text;
                    newTimer.Parity = cbParity.Text;
                    newTimer.StopBits = cbStopBits.Text;

                    context.Timers.Add(newTimer);
                    context.SaveChanges();
                    _alreadySaved = true;

                    // Disable fields
                    tbDescription.IsEnabled = false;
                    tbDate.IsEnabled = false;
                    cbScoringMethod.IsEnabled = false;
                    cbRounds.IsEnabled = false;
                    cbLanes.IsEnabled = false;
                    cbTimer.IsEnabled = false;
                    cbSerialPort.IsEnabled = false;
                    cbBaudRate.IsEnabled = false;
                    cbDataBits.IsEnabled = false;
                    cbParity.IsEnabled = false;
                    cbStopBits.IsEnabled = false;

                    // Enable/Disable buttons
                    btnSaveRaceSettings.IsEnabled = false;
                    btnEdit.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEditRaceSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Enable fields
                tbDescription.IsEnabled = true;
                tbDate.IsEnabled = true;
                cbScoringMethod.IsEnabled = true;
                cbRounds.IsEnabled = true;
                cbLanes.IsEnabled = true;
                cbTimer.IsEnabled = true;
                cbSerialPort.IsEnabled = true;
                cbBaudRate.IsEnabled = true;
                cbDataBits.IsEnabled = true;
                cbParity.IsEnabled = true;
                cbStopBits.IsEnabled = true;
                btnSaveRaceSettings.IsEnabled = true;

                btnEdit.IsEnabled = false;
                btnSaveRaceSettings.IsEnabled = true;

                tbDescription.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }      
    }
}
