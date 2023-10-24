namespace Sisa.Identity.Domain.AggregatesModel.SecurityEventAggregate;

public enum SecurityAction
{
    OTHER,
    REGISTER,
    LOGIN,
    LOGOUT,

    BLOCK,
    UNBLOCK,

    CONFIRM_EMAIL,
    CONFIRM_PHONE_NUMBER,

    CHANGE_USER_NAME,
    CHANGE_EMAIL,
    CHANGE_PASSWORD,
    RESET_PASSWORD,

    UPDATE_PERSONAL_INFO
}
