﻿using El_Booking.Model;
using El_Booking.Model.Repositories;
using El_Booking.Utility;
using El_Booking.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace El_Booking.Commands
{
    public class CreateUserCommand : CommandBase
    {

        private CreateUserViewModel _createUserViewModel { get; }
        private Storer _storer { get; } 

        public CreateUserCommand(CreateUserViewModel createUserViewModel, Storer storer)
        {
            _createUserViewModel = createUserViewModel;
            _storer = storer;

            _createUserViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        public override void Execute(object? parameter)
        {

                try
                {
                    User newUser = new User()
                    {
                        Email = _createUserViewModel.EnteredEmail,
                        TelephoneNumber = _createUserViewModel.EnteredPhoneNumber,
                        FirstName = _createUserViewModel.EnteredFirstName,
                        LastName = _createUserViewModel.EnteredLastName,
                        Password = _createUserViewModel.EnteredPassword,
                    };

                    _storer.UserRepository.Add(newUser);

                    MessageBox.Show($"Du er nu oprettet som bruger", "Succes", MessageBoxButton.OK);

                    _createUserViewModel.NavigateLoginCommand.Execute(parameter);

                }
                catch (NotSupportedException ex)
                {
                    MessageBox.Show("Ikke en gyldig email eller telefonnummer.", "Fejl", MessageBoxButton.OK);
                }
                catch (Microsoft.Data.SqlClient.SqlException ex)
                {
                    MessageBox.Show($"En bruger med: {_createUserViewModel.EnteredEmail} eller tlf.nr: {_createUserViewModel.EnteredPhoneNumber}, er allerede oprettet.", "Fejl", MessageBoxButton.OK);
                }
            }

        public override bool CanExecute(object? parameter)
        {
            if (
                !string.IsNullOrEmpty(_createUserViewModel.EnteredEmail) &&
                !string.IsNullOrEmpty(_createUserViewModel.EnteredPhoneNumber) &&
                !string.IsNullOrEmpty(_createUserViewModel.EnteredFirstName) &&
                !string.IsNullOrEmpty(_createUserViewModel.EnteredLastName) &&
                !string.IsNullOrEmpty(_createUserViewModel.EnteredPassword) &&
                _createUserViewModel.EnteredPassword == _createUserViewModel.EnteredPasswordAgain &&
                base.CanExecute(parameter)
                )
                return true;

            return false;
        }

    }
}
