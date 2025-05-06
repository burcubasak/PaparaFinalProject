using AutoMapper;
using FieldExpenseManager.FieldExpense.Application.DTOs.Expense;
using FieldExpenseManager.FieldExpense.Application.DTOs.ExpenseCategory;
using FieldExpenseManager.FieldExpense.Application.DTOs.ExpensePayment;
using FieldExpenseManager.FieldExpense.Application.DTOs.User;
using FieldExpenseManager.FieldExpense.Domain.Entities;

namespace FieldExpenseManager.FieldExpense.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
           // -----ExpenseCategory----// -
            CreateMap<ExpenseCategory, CategoryDto>();
            CreateMap<CreateCategoryDto, ExpenseCategory>();
            CreateMap<UpdateCategoryDto, ExpenseCategory>();

            //-----ExpenseAttachment-----//
            CreateMap<ExpenseAttachment, ExpenseAttachmentDto>();

            //-----ExpensePayment-----//
            CreateMap<ExpensePayments, ExpensePaymentDto>();

            //----User----//
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();

            //-----Expense-----//
            CreateMap<Expense, ExpenseDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.ExpenseCategory.Name))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.User.FirstName}  {src.User.LastName}"))
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments));

            CreateMap<CreateExpenseDto, Expense>();
        }
    }
}
