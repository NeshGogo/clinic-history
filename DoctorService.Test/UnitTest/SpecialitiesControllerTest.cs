using AutoMapper;
using DoctorService.Controllers;
using DoctorService.Data;
using DoctorService.Data.Repositories;
using DoctorService.Dtos;
using DoctorService.Entities;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DoctorService.Test.UnitTest
{
    public class SpecialitiesControllerTest : TestBase
    {
        private string _dbName;
        private AppDbContext _context;
        private IMapper _mapper;
        private Mock<IHttpContextAccessor> mockIHttpContextAccessor;

        [SetUp]
        public async Task Setup()
        {
            _dbName = Guid.NewGuid().ToString();
            _context = BuildContext(_dbName);
            _mapper = ConfigureAutoMapper();
            mockIHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockIHttpContextAccessor.Object.HttpContext = BuildHttpContext();
        }

        [Test]
        public void ShouldReturnAnEmptyArrayWhenThereAreNotSpecialitiesAtGetAll()
        {
            var repository = new SpecialityRepository(_context, mockIHttpContextAccessor.Object);

            var controller = new SpecialitiesController(repository, _mapper);
            var results = controller.Get();
            Assert.IsEmpty(results.Value);
        }

        [Test]
        public void ShouldGetAllSpecialities()
        {
            var initItems = new List<Speciality>
            {
                new Speciality{ Name = "Test1"},
                new Speciality{ Name = "Test2"},
                new Speciality{ Name = "Test3"},
            }.Select(p =>
            {
                p.Create(defaultUserEmail);
                return p;
            });
            var context2 = BuildContext(_dbName);
            context2.AddRange(initItems);
            context2.SaveChanges();
            var repository = new SpecialityRepository(_context, mockIHttpContextAccessor.Object);

            var controller = new SpecialitiesController(repository, _mapper);
            var results = controller.Get();
            Assert.IsNotEmpty(results.Value);
            Assert.AreEqual(initItems.Count(), results.Value.Count());
        }

        [Test]
        public void ShouldGetSpecialityById()
        {
            var initItems = new List<Speciality>
            {
                new Speciality{ Name = "Test1", Description = "Test description"},
                new Speciality{ Name = "Test2"},
                new Speciality{ Name = "Test3"},
            }.Select(p =>
            {
                p.Create(defaultUserEmail);
                return p;
            });
            var context2 = BuildContext(_dbName);
            context2.AddRange(initItems);
            context2.SaveChanges();
            var repository = new SpecialityRepository(_context, mockIHttpContextAccessor.Object);

            var controller = new SpecialitiesController(repository, _mapper);
            var valTester = _context.Set<Speciality>().First();
          
            var results = controller.GetById(valTester.Id);

            Assert.IsNotNull(results.Value);
            Assert.AreEqual(results.Value.Name, valTester.Name);
            Assert.AreEqual(results.Value.Description, valTester.Description);
        }

        [Test]
        public void ShouldReturnNullWhenCouldNotFoundASpecialityById()
        {
            var initItems = new List<Speciality>
            {
                new Speciality{ Name = "Test1", Description = "Test description"},
                new Speciality{ Name = "Test2"},
                new Speciality{ Name = "Test3"},
            }.Select(p =>
            {
                p.Create(defaultUserEmail);
                return p;
            });
            var context2 = BuildContext(_dbName);
            context2.AddRange(initItems);
            context2.SaveChanges();
            var repository = new SpecialityRepository(_context, mockIHttpContextAccessor.Object);

            var controller = new SpecialitiesController(repository, _mapper);
            var results = controller.GetById(Guid.NewGuid().ToString());

            Assert.IsNull(results.Value);
        }

        [Test]
        public async Task ShouldNotRegisterASpecialityBecauseAlreadyExists()
        {
            var initItems = new List<Speciality>
            {
                new Speciality{ Name = "Test1", Description = "Test description"},
                new Speciality{ Name = "Test2"},
                new Speciality{ Name = "Test3"},
            }.Select(p =>
            {
                p.Create(defaultUserEmail);
                return p;
            });
            _context.AddRange(initItems);
            _context.SaveChanges();
            var context2 = BuildContext(_dbName);
            
            var repository = new SpecialityRepository(context2, mockIHttpContextAccessor.Object);

            var controller = new SpecialitiesController(repository, _mapper);
            var createDto = new SpecialityCreateDto { Name = "Test1" };
            var results = await controller.Post(createDto);
            var value = (BadRequestObjectResult)results.Result;
            Assert.AreEqual(value.StatusCode, (int)HttpStatusCode.BadRequest);
            Assert.AreEqual(value.Value, $"Already exists a speciality with the name: {createDto.Name}");
        }

        [Test]
        public async Task ShouldRegisterASpeciality()
        {
            var httpcontext = new HttpContextAccessor();
            httpcontext.HttpContext = BuildHttpContext();
            var repository = new SpecialityRepository(_context, httpcontext);

            var controller = new SpecialitiesController(repository, _mapper);
            controller.ControllerContext = BuildControllerContext();
            var createDto = new SpecialityCreateDto { Name = "Test1" };
            var results = await controller.Post(createDto);
            var result = (CreatedAtRouteResult)results.Result;
            var value = (SpecialityDto)result.Value;
            
            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.Created);
            Assert.IsNotNull(value) ;
            Assert.AreEqual(value.Name, createDto.Name);
            var context2 = BuildContext(_dbName);
            var amount = context2.Set<Speciality>().Count();
            Assert.AreEqual(amount, 1);
        }

        [Test]
        public async Task ShouldNotUpdateASpecialityBecauseIdNotFound()
        {
            var httpcontext = new HttpContextAccessor();
            httpcontext.HttpContext = BuildHttpContext();
            var repository = new SpecialityRepository(_context, httpcontext);

            var controller = new SpecialitiesController(repository, _mapper);
            controller.ControllerContext = BuildControllerContext();
            var createDto = new SpecialityCreateDto { Name = "Test1" };
            var id = Guid.NewGuid().ToString();
            var result = (NotFoundObjectResult) await controller.Put(id, createDto);
            
            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.NotFound);
            Assert.AreEqual(result.Value, $"Could not get a speciality with id: {id}");
        }

        [Test]
        public async Task ShouldNotUpdateASpecialityBecauseAlreadyExists()
        {
            var initItems = new List<Speciality>
            {
                new Speciality{ Name = "Test1", Description = "Test description"},
                new Speciality{ Name = "Test2"},
                new Speciality{ Name = "Test3"},
            }.Select(p =>
            {
                p.Create(defaultUserEmail);
                return p;
            });
            _context.AddRange(initItems);
            _context.SaveChanges();
            var context2 = BuildContext(_dbName);

            var repository = new SpecialityRepository(context2, mockIHttpContextAccessor.Object);

            var controller = new SpecialitiesController(repository, _mapper);
            var createDto = new SpecialityCreateDto { Name = "Test1" };
            var id = context2.Set<Speciality>().Last().Id;
            var result = (BadRequestObjectResult) await controller.Put(id, createDto);
            
            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.BadRequest);
            Assert.AreEqual(result.Value, $"Already exists a speciality with the name: {createDto.Name}");
        }

        [Test]
        public async Task ShouldUpdateASpeciality()
        {
            var initItems = new List<Speciality>
            {
                new Speciality{ Name = "Test12", Description = "Test description"},
                new Speciality{ Name = "Test2"},
                new Speciality{ Name = "Test3"},
            }.Select(p =>
            {
                p.Create(defaultUserEmail);
                return p;
            });
            _context.AddRange(initItems);
            _context.SaveChanges();
            var context2 = BuildContext(_dbName);

            var httpcontext = new HttpContextAccessor();
            httpcontext.HttpContext = BuildHttpContext();
            var repository = new SpecialityRepository(context2, httpcontext);

            var controller = new SpecialitiesController(repository, _mapper);
            var createDto = new SpecialityCreateDto { Name = "Test1" };
            var id = context2.Set<Speciality>().Last().Id;
            var result = (NoContentResult)await controller.Put(id, createDto);

            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.NoContent);
            var context3 = BuildContext(_dbName);
            var value = context3.Set<Speciality>().FirstOrDefault(p => p.Id == id);
            Assert.IsNotNull(value);
            Assert.AreEqual(value.Name, createDto.Name);
        }


        [Test]
        public async Task ShouldNotActiveOrDisactiveASpecialityBecauseIdNotFound()
        {
            var httpcontext = new HttpContextAccessor();
            httpcontext.HttpContext = BuildHttpContext();
            var repository = new SpecialityRepository(_context, httpcontext);

            var controller = new SpecialitiesController(repository, _mapper);
            controller.ControllerContext = BuildControllerContext();

            var id = Guid.NewGuid().ToString();
            var result = (NotFoundObjectResult)await controller.ActiveOrDisactive(id);

            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.NotFound);
            Assert.AreEqual(result.Value, $"Could not get a speciality with id: {id}");
        }

        [Test]
        public async Task ShouldActiveASpeciality()
        {
            var entity = new Speciality { Name = "Test1", Description = "Test description" };
            entity.Create(defaultUserEmail);
            entity.ActiveOrDisable(defaultUserEmail);
            _context.Set<Speciality>().Add(entity);
            _context.SaveChanges();

            var context2 = BuildContext(_dbName);
            var httpcontext = new HttpContextAccessor();
            httpcontext.HttpContext = BuildHttpContext();

            Assert.IsFalse(entity.Active);
            var repository = new SpecialityRepository(context2, httpcontext);

            var controller = new SpecialitiesController(repository, _mapper);
            controller.ControllerContext = BuildControllerContext();
            var result = (NoContentResult)await controller.ActiveOrDisactive(entity.Id);

            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.NoContent);
            var context3 = BuildContext(_dbName);
            var dbEntity = context3.Set<Speciality>().First();
            Assert.IsTrue(dbEntity.Active);
        }

        [Test]
        public async Task ShouldDisactiveASpeciality()
        {
            var entity = new Speciality { Name = "Test1", Description = "Test description" };
            entity.Create(defaultUserEmail);
            _context.Set<Speciality>().Add(entity);
            _context.SaveChanges();

            var context2 = BuildContext(_dbName);
            var httpcontext = new HttpContextAccessor();
            httpcontext.HttpContext = BuildHttpContext();
            var repository = new SpecialityRepository(context2, httpcontext);

            var controller = new SpecialitiesController(repository, _mapper);
            controller.ControllerContext = BuildControllerContext();
            var result = (NoContentResult)await controller.ActiveOrDisactive(entity.Id);

            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.NoContent);
            var context3 = BuildContext(_dbName);
            var dbEntity = context3.Set<Speciality>().First();
            Assert.IsFalse(dbEntity.Active);
        }

        [Test]
        public async Task ShouldNotDeleteASpecialityBecauseIdNotFound()
        {
            var httpcontext = new HttpContextAccessor();
            httpcontext.HttpContext = BuildHttpContext();
            var repository = new SpecialityRepository(_context, httpcontext);

            var controller = new SpecialitiesController(repository, _mapper);
            controller.ControllerContext = BuildControllerContext();

            var id = Guid.NewGuid().ToString();
            var result = (NotFoundObjectResult)await controller.Delete(id);

            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.NotFound);
            Assert.AreEqual(result.Value, $"Could not get a speciality with id: {id}");
        }

        [Test]
        public async Task ShouldDeleteASpeciality()
        {
            var entity = new Speciality { Name = "Test1", Description = "Test description" };
            entity.Create(defaultUserEmail);
            _context.Set<Speciality>().Add(entity);
            _context.SaveChanges();

            var context2 = BuildContext(_dbName);
            var httpcontext = new HttpContextAccessor();
            httpcontext.HttpContext = BuildHttpContext();
            var repository = new SpecialityRepository(context2, httpcontext);

            var controller = new SpecialitiesController(repository, _mapper);
            controller.ControllerContext = BuildControllerContext();

            var result = (NoContentResult)await controller.Delete(entity.Id);

            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.NoContent);
            var context3 = BuildContext(_dbName);
            var dbEntity = context3.Set<Speciality>().FirstOrDefault();
            Assert.IsNull(dbEntity);
        }
    }
}
