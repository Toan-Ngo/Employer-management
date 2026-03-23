using System.ComponentModel.DataAnnotations;

namespace HRMS.Core.DTOs
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã xác nhận là bắt buộc.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Mã xác nhận phải gồm chính xác 6 ký tự.")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu mới là bắt buộc.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string NewPassword { get; set; } = string.Empty;
    }
}
