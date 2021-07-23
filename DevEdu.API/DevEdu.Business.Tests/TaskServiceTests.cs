using System;
using System.Collections.Generic;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class TaskServiceTests
    {
        private Mock<ITaskRepository> _taskRepoMock;
        private Mock<ICourseRepository> _courseRepoMock;
        private Mock<IStudentAnswerOnTaskRepository> _studentAnswerRepoMock;

        [SetUp]
        public void Setup()
        {
            _taskRepoMock = new Mock<ITaskRepository>();
            _courseRepoMock = new Mock<ICourseRepository>();
            _studentAnswerRepoMock = new Mock<IStudentAnswerOnTaskRepository>();
        }


        [Test]
        public void AddTask_SimpleDtoWithoutTags_TaskCreated()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithoutTags();

            _taskRepoMock.Setup(x => x.AddTask(taskDto)).Returns(TaskData.expectedTaskId);
            _taskRepoMock.Setup(x => x.AddTagToTask(It.IsAny<int>(), It.IsAny<int>()));

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object);

            //When
            var actualTaskId = sut.AddTask(taskDto);

            //Than
            Assert.AreEqual(TaskData.expectedTaskId, actualTaskId);
            _taskRepoMock.Verify(x => x.AddTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTask(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddTask_DtoWithTags_TaskWithTagsCreated()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithTags();

            _taskRepoMock.Setup(x => x.AddTask(taskDto)).Returns(TaskData.expectedTaskId);
            _taskRepoMock.Setup(x => x.AddTagToTask(TaskData.expectedTaskId, It.IsAny<int>()));

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object);

            //When
            var actualTaskId = sut.AddTask(taskDto);

            //Than
            Assert.AreEqual(TaskData.expectedTaskId, actualTaskId);
            _taskRepoMock.Verify(x => x.AddTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTask(TaskData.expectedTaskId, It.IsAny<int>()), Times.Exactly(taskDto.Tags.Count));
        }

        [Test]
        public void GetTaskById_IntTaskId_ReturnedTaskDto()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithTags();

            _taskRepoMock.Setup(x => x.GetTaskById(TaskData.expectedTaskId)).Returns(taskDto);

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object);

            //When
            var dto = sut.GetTaskById(TaskData.expectedTaskId);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(TaskData.expectedTaskId), Times.Once);
        }

        [Test]
        public void GetTaskById_WhenTaskDoesNotExist_EntityNotFoundException()
        {
            _taskRepoMock.Setup(x => x.GetTaskById(TaskData.expectedTaskId)).Throws(new EntityNotFoundException($"task with id = {TaskData.expectedTaskId} was not found"));

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object);

            EntityNotFoundException ex = Assert.Throws<EntityNotFoundException>(
                () => sut.GetTaskById(TaskData.expectedTaskId));
            Assert.That(ex.Message, Is.EqualTo($"task with id = {TaskData.expectedTaskId} was not found"));

            _taskRepoMock.Verify(x => x.GetTaskById(TaskData.expectedTaskId), Times.Once);
        }
        

        [Test]
        public void GetTaskWithCoursesById_IntTaskId_ReturnedTaskDtoWithCourses()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithTags();

            var courseDtos = TaskData.GetListOfCourses();

            _taskRepoMock.Setup(x => x.GetTaskById(TaskData.expectedTaskId)).Returns(taskDto);
            _courseRepoMock.Setup(x => x.GetCoursesToTaskByTaskId(TaskData.expectedTaskId)).Returns(courseDtos);
            taskDto.Courses = courseDtos;

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object);

            //When
            var dto = sut.GetTaskWithCoursesById(TaskData.expectedTaskId);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(TaskData.expectedTaskId), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesToTaskByTaskId(TaskData.expectedTaskId), Times.Once);
        }

        [Test]
        public void GetTaskWithCoursesById_WhenTaskDoesNotExist_EntityNotFoundException()
        {
            _taskRepoMock.Setup(x => x.GetTaskById(TaskData.expectedTaskId)).Throws(new EntityNotFoundException($"task with id = {TaskData.expectedTaskId} was not found"));

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object);

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo($"task with id = {TaskData.expectedTaskId} was not found"), 
                () => sut.GetTaskWithCoursesById(TaskData.expectedTaskId));
            _taskRepoMock.Verify(x => x.GetTaskById(TaskData.expectedTaskId), Times.Once);
        }

        [Test]
        public void GetTaskWithAnswersById_IntTaskId_ReturnedTaskDtoWithStudentAnswers()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithTags();

            var studentAnswersDtos = TaskData.GetListOfStudentAnswers();

            _taskRepoMock.Setup(x => x.GetTaskById(TaskData.expectedTaskId)).Returns(taskDto);
            _studentAnswerRepoMock.Setup(x => x.GetStudentAnswersToTaskByTaskId(TaskData.expectedTaskId)).Returns(studentAnswersDtos);
            taskDto.StudentAnswers = studentAnswersDtos;

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object);

            //When
            var dto = sut.GetTaskWithAnswersById(TaskData.expectedTaskId);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(TaskData.expectedTaskId), Times.Once);
            _studentAnswerRepoMock.Verify(x => x.GetStudentAnswersToTaskByTaskId(TaskData.expectedTaskId), Times.Once);
        }

        [Test]
        public void GetTaskWithAnswersById_WhenTaskDoesNotExist_EntityNotFoundException()
        {
            _taskRepoMock.Setup(x => x.GetTaskById(TaskData.expectedTaskId)).Throws(new EntityNotFoundException($"task with id = {TaskData.expectedTaskId} was not found"));

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object);

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo($"task with id = {TaskData.expectedTaskId} was not found"), 
                () => sut.GetTaskWithAnswersById(TaskData.expectedTaskId));
            _taskRepoMock.Verify(x => x.GetTaskById(TaskData.expectedTaskId), Times.Once);
        }

        [Test]
        public void GetTasks_NoEntry_ReturnedTaskDtos()
        {
            //Given
            var taskDtos = TaskData.GetListOfTasks();

            _taskRepoMock.Setup(x => x.GetTasks()).Returns(taskDtos);

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object);

            //When
            var dtos = sut.GetTasks();

            //Than
            Assert.AreEqual(taskDtos, dtos);
            _taskRepoMock.Verify(x => x.GetTasks(), Times.Once);
        }

        [Test]
        public void GetTasks_WhenDoesNotExistAnyTask_EntityNotFoundException()
        {
            _taskRepoMock.Setup(x => x.GetTasks()).Throws(new EntityNotFoundException($"not found any task"));

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object);

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo($"not found any task"), () => sut.GetTasks());
            _taskRepoMock.Verify(x => x.GetTasks(), Times.Once);
        }

        [Test]
        public void UpdateTask_TaskDto_ReturnUpdateTaskDto()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithTags();
            var expectedTaskDto = TaskData.GetAnotherTaskDtoWithTags();

            //_taskRepoMock.Setup(x => x.UpdateTask(taskDto));
            _taskRepoMock.Setup(x => x.GetTaskById(taskDto.Id)).Returns(expectedTaskDto);

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object);

            //When
            var actualTaskDto = sut.UpdateTask(taskDto);

            //Then
            Assert.AreEqual(expectedTaskDto, actualTaskDto);
            _taskRepoMock.Verify(x => x.UpdateTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.GetTaskById(taskDto.Id), Times.Exactly(2));
        }

        [Test]
        public void UpdateTask_WhenTaskDoesNotExist_EntityNotFoundException()
        {
            var taskDto = TaskData.GetTaskDtoWithTags();
            _taskRepoMock.Setup(x => x.UpdateTask(taskDto)).Throws(new EntityNotFoundException($"task with id = {TaskData.expectedTaskId} was not found"));

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object);
            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                    .And.Message.EqualTo($"task with id = {TaskData.expectedTaskId} was not found"),
                () => sut.UpdateTask(taskDto));
        }

        [Test]
        public void DeleteTask_WhenTaskDoesNotExist_EntityNotFoundException()
        {
            _taskRepoMock.Setup(x => x.DeleteTask(TaskData.expectedTaskId)).Throws(new EntityNotFoundException($"task with id = {TaskData.expectedTaskId} was not found"));

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object);

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                    .And.Message.EqualTo($"task with id = {TaskData.expectedTaskId} was not found"),
                () => sut.DeleteTask(TaskData.expectedTaskId)); 
            _taskRepoMock.Verify(x => x.DeleteTask(TaskData.expectedTaskId), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskById(TaskData.expectedTaskId), Times.Exactly(1));
        }


        [Test]
        public void GetGroupsByTaskId_IntTaskId_ReturnedListOfGroupTaskDtoWithTask()
        {
            //Given
            var groupTaskList = GroupTaskData.GetListOfGroupTaskDtoWithGroup();
            const int taskId = GroupTaskData.TaskId;

            _taskRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupTaskList);

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object);

            //When
            var dto = sut.GetGroupsByTaskId(taskId);

            //Than
            Assert.AreEqual(groupTaskList, dto);
            _taskRepoMock.Verify(x => x.GetGroupsByTaskId(taskId), Times.Once);
        }
    }
}