﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AldeiaParental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using AldeiaParental.Data;

namespace AldeiaParental.Areas.Identity.Pages.Account.Manage
{
    public partial class ProfileModel : PageModel
    {
        private readonly UserManager<AldeiaParentalUser> _userManager;
        private readonly SignInManager<AldeiaParentalUser> _signInManager;
        private readonly RoleManager<AldeiaParentalRole> _roleManager;
        private readonly ILogger<ProfileModel> _logger;
        private readonly ApplicationDbContext _context;

        private const string _customerRole = "Cliente";
        private const string _caregiverRole = "Cuidador";

        public ProfileModel(
            UserManager<AldeiaParentalUser> userManager,
            RoleManager<AldeiaParentalRole> roleManager,
            SignInManager<AldeiaParentalUser> signInManager,
            ILogger<ProfileModel> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _context = context;
        }

        public string Username { get; set; }
        public bool ValidDocs { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Telefone")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Cliente")]
            public bool Customer { get; set; }
            [Display(Name = "Cuidador")]
            public bool Caregiver { get; set; }
            [Display(Name = "Endereço")]
            public string Address { get; set; }
            [Required]
            [Display(Name = "Nome")]
            public string  FirstName { get; set; }
            [Required]
            [Display(Name = "Sobrenome")]
            public string LastName { get; set; }
        }

        private async Task LoadAsync(AldeiaParentalUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var caregiver = await _userManager.IsInRoleAsync(user, _caregiverRole);
            var customer = await _userManager.IsInRoleAsync(user, _customerRole);

            Username = userName;
            ValidDocs = _context.PersonalDocument.Any(d => d.UserId.Equals(user.Id) && (d.Valid??false));
            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Caregiver = caregiver,
                Customer = customer,
                Address = user.Address,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage+= "Unexpected error when trying to set phone number.";
                }
            }
            
            if (Input.Address != user.Address)
            {
                user.Address = Input.Address;

                var updateAddress = await _userManager.UpdateAsync(user);
                if (!updateAddress.Succeeded)
                {
                    StatusMessage += "Unexpected error when trying to set Address.";
                }
       
            }
            bool validDocs = _context.PersonalDocument.Any(d => d.UserId.Equals(user.Id) && (d.Valid??false));
            if (!validDocs && Input.FirstName != user.FirstName)
            {
                user.FirstName = Input.FirstName;

                var updateFirstName = await _userManager.UpdateAsync(user);
                if (!updateFirstName.Succeeded)
                {
                    StatusMessage += "Unexpected error when trying to set FirsName.";
                }
       
            }
            if (!validDocs && Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;

                var updateLastName = await _userManager.UpdateAsync(user);
                if (!updateLastName.Succeeded)
                {
                    StatusMessage += "Unexpected error when trying to set LastName.";
                }
       
            }

            #region set roles
            if (Input.Customer)
            {

                bool customerRoleFound = await _roleManager.RoleExistsAsync(_customerRole);
                if (!customerRoleFound)
                {
                    _logger.LogWarning("Could not set role for " + user.UserName
                    + " The role " + _customerRole + "does not exist");
                    StatusMessage += "Role does not exists:" + _customerRole;
                }
                else
                {
                    bool hasRoleAlready = await _userManager.IsInRoleAsync(user, _customerRole);
                    if (!hasRoleAlready)
                    {
                        try
                        {
                            await _userManager.AddToRoleAsync(user, _customerRole);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning("Could not set " + _customerRole + " role for " + user.UserName
                        + " " + ex.Message);
                            StatusMessage += "Error setting Role:" + _customerRole;
                        }
                    }

                }
            }
            else
            {
                bool hasRole = await _userManager.IsInRoleAsync(user, _customerRole);
                if (hasRole)
                {
                    try
                    {
                        await _userManager.RemoveFromRoleAsync(user, _customerRole);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning("Could not remove " + _customerRole + "from role for " + user.UserName
                    + " " + ex.Message);
                        StatusMessage += "Could not remove Role:" + _customerRole;
                    }
                }

            }

            if (Input.Caregiver)
            {

                bool caregiverRoleFound = await _roleManager.RoleExistsAsync(_caregiverRole);
                if (!caregiverRoleFound)
                {
                    _logger.LogWarning("Could not set role for " + user.UserName
                    + " The role " + _caregiverRole + "does not exist");
                    StatusMessage += "Role does not exists:" + _caregiverRole;
                }
                else
                {
                    bool hasRoleAlready = await _userManager.IsInRoleAsync(user, _caregiverRole);
                    if (!hasRoleAlready)
                    {
                        try
                        {
                            await _userManager.AddToRoleAsync(user, _caregiverRole);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning("Could not set " + _caregiverRole + " role for " + user.UserName
                        + " " + ex.Message);
                            StatusMessage += "Could not set Role:" + _caregiverRole;
                        }
                    }

                }
            }
            else
            {
                bool hasRole = await _userManager.IsInRoleAsync(user, _caregiverRole);
                if (hasRole)
                {
                    try
                    {
                        await _userManager.RemoveFromRoleAsync(user, _caregiverRole);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning("Could not remove " + _caregiverRole + "from role for " + user.UserName
                    + " " + ex.Message);
                        StatusMessage += "Could not remove Role:" + _caregiverRole;
                    }
                }

            }
            #endregion



            await _signInManager.RefreshSignInAsync(user);
            if (StatusMessage!=null)
            {
                _logger.LogWarning(StatusMessage);
            }
            else
            {
                StatusMessage = "Perfil Atualizado.";
            }
            return RedirectToPage();
        }
    }
}
