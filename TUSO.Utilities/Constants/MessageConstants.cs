/*
 * Created by: Labib
 * Date created: 31.08.2022
 * Last modified: 08.11.2022
 * Modified by: Bithy
 */
namespace TUSO.Utilities.Constants
{
    public static class MessageConstants
    {
        public const string GenericError = "Something went wrong, please try after sometimes! If you are experiencing similar frequently, please report it to helpdesk.";

        public const string RequiredFieldError = "Required!";

        public const string WrongPasswordError = "Wrong Password";

        public const string PasswordLengthError = "Password must have atleast 5 characters!";

        public const string GenderNotSelectedError = "Gender was not selected!";

        public const string UnofficialEmailAddressError = "Only IHM’s official email address is allowed!";

        public const string UnderAgedUserError = "User must be equal or above 18 years old!";

        public const string InvalidDOBError = "Date of birth cannot be a future date!";

        public const string ConfirmedPasswordNotMatchedError = "Confirmed password did not matched!";

        public const string DuplicateUserAccountError = "Username already exists!";

        public const string DuplicateCellphoneError = "The cellphone number is associated with another user account!";

        public const string DuplicateError = "Duplicate data found!";

        public const string DependencyError = "This record cannot be deleted. It is already in use.";

        public const string NoMatchFoundError = "No match found!";

        public const string InvalidParameterError = "Invalid parameter(s)!";

        public const string UnauthorizedAttemptOfRecordUpdateError = "Unauthorized attempt of updating record!";

        public const string IfNotAlphabet = "Input string is not correct format!";

        public const string IfNotInteger = "Input string is not correct format!";

        public const string IfNotSelected = "Choose an option!";

        public const string IfNotEmailAddress = "Email is not correct formation!";

        public const string IfNotCountryCode = "Invalid counrty code!";

        public const string IfFutureDateSelected = "Please select valid date. Your can't select future date!";

        public const string OpenTicketFoundUnderSystemError = "System can't be deleted, there is open ticket under the system.";

        public const string OpenTicketFoundUnderTeamError = "This Team Has Opened Tickets, Can't Delete this Team.";

        public const string UpdateMessage = "Record Successfully Updated";

        public const string SaveMessage = "Record Successfully Saved";

        public const string DeleteMessage = "Record Successfully Deleted";
    }
}