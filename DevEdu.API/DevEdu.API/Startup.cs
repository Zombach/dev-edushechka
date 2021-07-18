using System;
using System.Linq;
using System.Text;
using DevEdu.API.Configuration;
using DevEdu.Business.Services;
using DevEdu.Business.Services;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DevEdu.Business.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;

namespace DevEdu.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IMaterialRepository, MaterialRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IStudentAnswerOnTaskRepository, StudentAnswerOnTaskRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<ITopicService, TopicService>();

            services.AddControllers();

            services.AddMvc();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    //���� ����� false, �� SSL ��� �������� ������ �� ������������. ������ ������ ������� ���������� ������ �� ������������.
                    //� �������� ���������� ��� �� ����� ������������ �������� ������ �� ��������� https.
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // ��������, ����� �� �������������� �������� ��� ��������� ������
                        ValidateIssuer = true,
                        // ������, �������������� ��������
                        ValidIssuer = AuthOptions.ISSUER,
                        // ����� �� �������������� ����������� ������
                        ValidateAudience = true,
                        // ��������� ����������� ������
                        ValidAudience = AuthOptions.AUDIENCE,
                        // ����� �� �������������� ����� �������������
                        ValidateLifetime = true,
                        // ��������� ����� ������������
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        // ��������� ����� ������������
                        ValidateIssuerSigningKey = true,
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminAuthorizationPolicy",
                    policy =>
                    {
                        policy.RequireClaim("DeveloperBoss", "IAmBoss");
                        policy.RequireRole("Teacher");
                    });
            });

            services.AddSwaggerDocument(document =>
            {
                document.DocumentName = "Endpoints for DevEdu";
                document.Title = "DevEdu Education API";
                document.Version = "v8";
                document.Description = "An interface for some software.";

                document.DocumentProcessors.Add(
                    new SecurityDefinitionAppender("JWT token", new NSwag.OpenApiSecurityScheme
                    {
                        Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        Description = "Copy 'Bearer ' + valid JWT token into field",
                        In = NSwag.OpenApiSecurityApiKeyLocation.Header
                    }));
                document.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT token"));
            });

            // Add framework services.
            services.AddOptions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseOpenApi();
                app.UseSwaggerUi3();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}