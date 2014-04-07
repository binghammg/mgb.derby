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
using System.IO.Ports;

namespace mgb.derby
{    
    public partial class Configuration : Window
    {
        private Boolean _alreadySaved;
        private Race _savedRace;
        private Timer _savedTimer;

        public Configuration()
        {
            InitializeComponent();
            _savedRace = new Race();
            _savedTimer = new Timer();
        }

        private void Config_Loaded(object sender, RoutedEventArgs e)
        {               
            btnEditRaceSettings.IsEnabled = false;
            btnSaveRaceSettings.IsEnabled = true;
            _alreadySaved = false;

            //String[] serialPorts = SerialPort.GetPortNames();

            tbDescription.Focus();
        }

        private void btnSaveRaceSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new DerbyDBEntities())
                {
                    if (context.Database.Connection.State != ConnectionState.Open)
                    {
                        context.Database.Connection.Open();
                    }

                    // Create the race
                    Race theRace = null;

                    if (_alreadySaved == false)
                    {
                        theRace = new Race();
                    }
                    else
                    {
                        theRace = _savedRace;
                        context.Races.Attach(theRace);
                    }

                    theRace.Description = tbDescription.Text;
                    theRace.Date = Convert.ToDateTime(tbDate.Text);
                    theRace.ScoringMethod = cbScoringMethod.Text;
                    theRace.Rounds = Convert.ToInt32(cbRounds.Text);
                    theRace.Lanes = Convert.ToInt32(cbLanes.Text);

                    if (_alreadySaved == false)
                    {
                        context.Races.Add(theRace);
                    }
                    
                    context.SaveChanges();    //TODO: Figure out how to get the identity from the database to use in updates!  Otherwise it is zero and I get an error

                    _savedRace.Description = theRace.Description;
                    _savedRace.Date = theRace.Date;
                    _savedRace.ScoringMethod = theRace.ScoringMethod;
                    _savedRace.Rounds = theRace.Rounds;
                    _savedRace.Lanes = theRace.Lanes;

                    // Create the Timer
                    Timer theTimer = null;

                    if (_alreadySaved == false)
                    {
                        theTimer = new Timer();
                    }
                    else
                    {
                        theTimer = _savedTimer;
                        context.Timers.Attach(theTimer);
                    }

                    theTimer.RaceId = theRace.RaceID;
                    theTimer.Description = cbTimer.Text;
                    theTimer.SerialPort = cbSerialPort.Text;
                    theTimer.BaudRate = cbBaudRate.Text;
                    theTimer.DataBits = cbDataBits.Text;
                    theTimer.Parity = cbParity.Text;
                    theTimer.StopBits = cbStopBits.Text;

                    if (_alreadySaved == false)
                    {
                        context.Timers.Add(theTimer);
                    }

                    context.SaveChanges();
                    _alreadySaved = true;

                    _savedTimer.RaceId = theTimer.RaceId;
                    _savedTimer.Description = theTimer.Description;
                    _savedTimer.SerialPort = theTimer.SerialPort;
                    _savedTimer.BaudRate = theTimer.BaudRate;
                    _savedTimer.DataBits = theTimer.DataBits;
                    _savedTimer.Parity = theTimer.Parity;
                    _savedTimer.StopBits = theTimer.StopBits;

                    
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
                    btnEditRaceSettings.IsEnabled = true;
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

                btnEditRaceSettings.IsEnabled = false;
                btnSaveRaceSettings.IsEnabled = true;

                tbDescription.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnStartRace_Click(object sender, RoutedEventArgs e)
        {   
            //TODO: Start this window in its own thread.....
            wRace RaceWindow = new wRace();
            RaceWindow.ShowDialog();
        }      
    }
}
