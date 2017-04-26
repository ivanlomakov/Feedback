using System.Data.Entity.Migrations;
using Conference.Core.Entities.Questions;
using Conference.Core.Entities.Questions.Options;

namespace Conference.DAL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ConferenceContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ConferenceContext context)
        {
            this.CreateAcquaintanceQuestion(context);
            this.CreateIntroduceQuestion(context);
            this.CreateMeetBeforeQuestion(context);
            this.CreateTodayOpinionQuestion(context);
            this.CreateTodayRegistrationOpinionQuestion(context);
            this.CreateTodayCoffeOpinionQuestion(context);
            this.CreateTodayFirstReportsOpinionQuestion(context);
            this.CreateTodaySecondReportsOpinionQuestion(context);
            this.CreateTodayThirdReportsOpinionQuestion(context);
            this.ILikedQuestion(context);
            this.SuggestionQuestion(context);
            this.MeetAgainQuestion(context);

            context.SaveChanges();
        }

        private void CreateAcquaintanceQuestion(ConferenceContext context)
        {
            var question = new AcquaintanceQuestion
            {
                Order = 1,
                Text = string.Empty,
            };
            context.Questions.AddOrUpdate(question);
        }

        private void CreateIntroduceQuestion(ConferenceContext context)
        {
            var options = new QuestionOption[]
                {
                    new QuestionOption { Text = "Я студент/студентка" },
                    new QuestionOption { Text = "Я совмещаю учебу и фриланс" },
                    new QuestionOption { Text = "Я совмещаю учебу с работой в ИТ-компании" },
                    new QuestionOption { Text = "Я окончил/окончила учебу и активно ищу работу" },
                    new QuestionOption { Text = "Я работаю в ИТ-компании" },
                    new QuestionOption { Text = "Я фрилансер" },
                    new InputQuestionOption { Text = "Другое" }
                };

            var question = new SingleOptionQuestion
            {
                Order = 2,
                Text = "Для начала давай познакомимся. Выбери, пожалуйста, подходящий вариант из следующих утверждений:",
                Options = options
            };
            context.Questions.AddOrUpdate(question);
        }

        private void CreateMeetBeforeQuestion(ConferenceContext context)
        {
            var redirectQuestion = new TextQuestion
            {
                Text = "Что привело тебя на еще одну встречу {ConferenceName}?",
            };
            context.Questions.AddOrUpdate(redirectQuestion);

            var options = new QuestionOption[]
                {
                    new QuestionOption { Text = "Сегодня я пришел/пришла на встречу {ConferenceName} впервые" },
                    new RedirectionQuestionOption { Text = "Я уже бывал/бывала на встречах {ConferenceName}", Redirect = redirectQuestion }
                };

            var question = new SingleOptionQuestion
            {
                Order = 3,
                Text = "Теперь давай вспомним, виделись ли мы раньше:",
                Options = options
            };
            context.Questions.AddOrUpdate(question);
        }

        private void CreateTodayOpinionQuestion(ConferenceContext context)
        {
               var question = new RatingQuestion
            {
                Order = 4,
                Text = "Как ты оцениваешь сегодняшнюю встречу {ConferenceName}?",
                Options = new[] { new RatingOptionQuestion { From = 1, To = 5, Step = 1 } }
            };
            context.Questions.AddOrUpdate(question);
        }

        private void CreateTodayRegistrationOpinionQuestion(ConferenceContext context)
        {
            var question = new RatingQuestion
            {
                Order = 5,
                Text = "Что ты скажешь об организации регистрации?",
                Options = new[] { new RatingOptionQuestion { From = 1, To = 5, Step = 1 } }
            };
            context.Questions.AddOrUpdate(question);
        }

        private void CreateTodayCoffeOpinionQuestion(ConferenceContext context)
        {
            var question = new RatingQuestion
            {
                Order = 6,
                Text = "Что ты скажешь об организации кофе-брейка?",
                Options = new[] { new RatingOptionQuestion { From = 1, To = 5, Step = 1 } }
            };
            context.Questions.AddOrUpdate(question);
        }

        private void CreateTodayFirstReportsOpinionQuestion(ConferenceContext context)
        {
            var question = new RatingQuestion
            {
                Order = 7,
                Text = "Что ты скажешь о докладе Владимира Чмужа “ASP.NET Core JavaScript Services: интеграция ASP.NET c современными " 
                            + "frontend фреймворками и инструментами”:",
                Options = new[]
                                {
                                    new RatingOptionQuestion { Description = "Актуальность", From = 1, To = 5, Step = 1 },
                                    new RatingOptionQuestion { Description = "Информативность", From = 1, To = 5, Step = 1 },
                                    new RatingOptionQuestion { Description = "Подача информации спикером", From = 1, To = 5, Step = 1 }
                                }
            };
            context.Questions.AddOrUpdate(question);
        }

        private void CreateTodaySecondReportsOpinionQuestion(ConferenceContext context)
        {
            var question = new RatingQuestion
            {
                Order = 8,
                Text = "Что ты скажешь о докладе Виктора Кудри “Xamarin: наши первые шаги”:",
                Options = new[]
                                {
                                    new RatingOptionQuestion { Description = "Актуальность", From = 1, To = 5, Step = 1 },
                                    new RatingOptionQuestion { Description = "Информативность", From = 1, To = 5, Step = 1 },
                                    new RatingOptionQuestion { Description = "Подача информации спикером", From = 1, To = 5, Step = 1 }
                                }
            };
            context.Questions.AddOrUpdate(question);            
        }

        private void CreateTodayThirdReportsOpinionQuestion(ConferenceContext context)
        {
            var question = new RatingQuestion
            {
                Order = 9,
                Text = "Что ты скажешь о докладе Ивана Ломакова “SQL'фобия — первые симптомы и профилактика”:",
                Options = new[]
                                {
                                    new RatingOptionQuestion { Description = "Актуальность", From = 1, To = 5, Step = 1 },
                                    new RatingOptionQuestion { Description = "Информативность", From = 1, To = 5, Step = 1 },
                                    new RatingOptionQuestion { Description = "Подача информации спикером", From = 1, To = 5, Step = 1 }
                                }
            };
            context.Questions.AddOrUpdate(question);
        }

        private void ILikedQuestion(ConferenceContext context)
        {
            var question = new TextQuestion
            {
                Order = 10,
                Text = "На {ConferenceName} #{CurrentNumber} мне понравилось",
            };
            context.Questions.AddOrUpdate(question);
        }

        private void SuggestionQuestion(ConferenceContext context)
        {
            var question = new TextQuestion
            {
                Order = 11,
                Text = "Для того, чтобы встречи {ConferenceName}-комьюнити стали еще лучше и интереснее, я бы хотел/хотела предложить",
            };
            context.Questions.AddOrUpdate(question);
        }

        private void MeetAgainQuestion(ConferenceContext context)
        {
            var options = new QuestionOption[]
                {
                    new QuestionOption { Text = "Я обязательно приду на {ConferenceName} #{NextNumber}" },
                    new QuestionOption { Text = "Я не приду на {ConferenceName} #{NextNumber}" },
                    new QuestionOption { Text = "Я не знаю" }
                };

            var question = new SingleOptionQuestion
            {
                Order = 12,
                Text = "Мы встретимся в следующий раз?",
                Options = options
            };
            context.Questions.AddOrUpdate(question);
        }
    }
}