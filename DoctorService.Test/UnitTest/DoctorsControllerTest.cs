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
    public class DoctorsControllerTest : TestBase
    {
        private string _dbName;
        private AppDbContext _context;
        private IMapper _mapper;
        private Mock<IHttpContextAccessor> mockIHttpContextAccessor;
        private User _user;
        private Speciality _speciality;

        [SetUp]
        public async Task Setup()
        {
            _dbName = Guid.NewGuid().ToString();
            _context = BuildContext(_dbName);
            _mapper = ConfigureAutoMapper();
            mockIHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockIHttpContextAccessor.Object.HttpContext = BuildHttpContext();
            var dependentValues = AddDependentValues();
            _user = dependentValues.User;
            _speciality = dependentValues.Speciality;
        }

        [Test]
        public void ShouldReturnAnEmptyArrayWhenThereAreNotDoctorsAtGetAll()
        {
            var repository = new DoctorRepository(_context, mockIHttpContextAccessor.Object);

            var controller = new DoctorsController(repository, _mapper);
            var results = controller.GetAll();
            Assert.IsEmpty(results.Value);
        }

        [Test]
        public void ShouldGetAllDoctors()
        {
            var initItems = new List<Doctor>
            {
                new Doctor{ FullName = "Test1", Identification = "12345678901", User = _user, Speciality = _speciality},
                new Doctor{ FullName = "Test2", Identification = "12345678902", User = _user, Speciality = _speciality},
                new Doctor{ FullName = "Test3", Identification = "12345678903", User = _user, Speciality = _speciality},
            }.Select(p =>
            {
                p.Create(defaultUserEmail);
                return p;
            });            
            _context.AddRange(initItems);
            _context.SaveChanges();

            var context = BuildContext(_dbName);
            var repository = new DoctorRepository(context, mockIHttpContextAccessor.Object);

            var controller = new DoctorsController(repository, _mapper);
            var results = controller.GetAll();
            Assert.IsNotEmpty(results.Value);
            Assert.AreEqual(initItems.Count(), results.Value.Count());
        }

        [Test]
        public void ShouldGetDoctorById()
        {
            var initItems = new List<Doctor>
            {
                new Doctor{ FullName = "Test1", Identification = "12345678901", User = _user, Speciality = _speciality},
                new Doctor{ FullName = "Test2", Identification = "12345678902", User = _user, Speciality = _speciality},
                new Doctor{ FullName = "Test3", Identification = "12345678903", User = _user, Speciality = _speciality},
            }.Select(p =>
            {
                p.Create(defaultUserEmail);
                return p;
            });
            _context.AddRange(initItems);
            _context.SaveChanges();

            var context2 = BuildContext(_dbName);
            var repository = new DoctorRepository(context2, mockIHttpContextAccessor.Object);

            var controller = new DoctorsController(repository, _mapper);
            var valTester = _context.Set<Doctor>().First();
          
            var results = controller.GetById(valTester.Id);

            Assert.IsNotNull(results.Value);
            Assert.AreEqual(results.Value.FullName, valTester.FullName);
            Assert.AreEqual(results.Value.Identification, valTester.Identification);
        }

        [Test]
        public void ShouldReturnNullWhenCouldNotFoundADoctorById()
        {
            var initItems = new List<Doctor>
            {
                new Doctor{ FullName = "Test1", Identification = "12345678901", User = _user, Speciality = _speciality},
                new Doctor{ FullName = "Test2", Identification = "12345678902", User = _user, Speciality = _speciality},
                new Doctor{ FullName = "Test3", Identification = "12345678903", User = _user, Speciality = _speciality},
            }.Select(p =>
            {
                p.Create(defaultUserEmail);
                return p;
            });
            _context.AddRange(initItems);
            _context.SaveChanges();

            var context = BuildContext(_dbName);
            var repository = new DoctorRepository(context, mockIHttpContextAccessor.Object);

            var controller = new DoctorsController(repository, _mapper);
            var results = controller.GetById(Guid.NewGuid().ToString());

            Assert.IsNull(results.Value);
        }

        [Test]
        public async Task ShouldNotRegisterADoctorBecauseAlreadyExists()
        {
            var initItems = new List<Doctor>
            {
                new Doctor{ FullName = "Test1", Identification = "12345678901", User = _user, Speciality = _speciality},
                new Doctor{ FullName = "Test2", Identification = "12345678902", User = _user, Speciality = _speciality},
                new Doctor{ FullName = "Test3", Identification = "12345678903", User = _user, Speciality = _speciality},
            }.Select(p =>
            {
                p.Create(defaultUserEmail);
                return p;
            });
            _context.AddRange(initItems);
            _context.SaveChanges();

            var context2 = BuildContext(_dbName);            
            var repository = new DoctorRepository(context2, mockIHttpContextAccessor.Object);

            var controller = new DoctorsController(repository, _mapper);
            var createDto = new DoctorCreateDto { FullName = "Test1", Identification = "12345678901", SpecialityId = _speciality.Id};
            var results = await controller.Post(createDto);
            var value = (BadRequestObjectResult)results.Result;

            Assert.AreEqual(value.StatusCode, (int)HttpStatusCode.BadRequest);
            Assert.AreEqual(value.Value, $"Already exists a doctor with the identification: {createDto.Identification}");
        }

        [Test]
        public async Task ShouldRegisterADoctor()
        {
            var httpcontext = new HttpContextAccessor();
            httpcontext.HttpContext = BuildHttpContext();
            var repository = new DoctorRepository(_context, httpcontext);

            var controller = new DoctorsController(repository, _mapper);
            controller.ControllerContext = BuildControllerContext();
            var createDto = new DoctorCreateDto { FullName = "Test1", Identification = "12345678901", SpecialityId = _speciality.Id };
            var results = await controller.Post(createDto);
            var result = (CreatedAtRouteResult)results.Result;
            var value = (DoctorDto)result.Value;
            
            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.Created);
            Assert.IsNotNull(value);
            var context2 = BuildContext(_dbName);
            var dbEntity = context2.Set<Doctor>().FirstOrDefault();
            Assert.IsNotNull(dbEntity);

            Assert.AreEqual(value.FullName, createDto.FullName);
            Assert.AreEqual(dbEntity.FullName, createDto.FullName);

            Assert.AreEqual(value.Identification, createDto.Identification);
            Assert.AreEqual(dbEntity.Identification, createDto.Identification);

            Assert.AreEqual(value.SpecialityId, createDto.SpecialityId);
            Assert.AreEqual(dbEntity.SpecialityId, createDto.SpecialityId);
        }

        [Test]
        public async Task ShouldNotUpdateADoctorBecauseIdNotFound()
        {
            var httpcontext = new HttpContextAccessor();
            httpcontext.HttpContext = BuildHttpContext();
            var repository = new DoctorRepository(_context, httpcontext);

            var controller = new DoctorsController(repository, _mapper);
            controller.ControllerContext = BuildControllerContext();
            var createDto = new DoctorCreateDto { FullName = "Test1", Identification = "12345678901", SpecialityId = _speciality.Id };
            var id = Guid.NewGuid().ToString();
            var result = (NotFoundObjectResult) await controller.Put(id, createDto);
            
            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.NotFound);
            Assert.AreEqual(result.Value, $"Could not get a doctor with id: {id}");
        }

        [Test]
        public async Task ShouldNotUpdateADoctorBecauseAlreadyExists()
        {
            var initItems = new List<Doctor>
            {
                new Doctor{ FullName = "Test1", Identification = "12345678901", User = _user, Speciality = _speciality},
                new Doctor{ FullName = "Test2", Identification = "12345678902", User = _user, Speciality = _speciality},
                new Doctor{ FullName = "Test3", Identification = "12345678903", User = _user, Speciality = _speciality},
            }.Select(p =>
            {
                p.Create(defaultUserEmail);
                return p;
            });
            _context.AddRange(initItems);
            _context.SaveChanges();
            var context2 = BuildContext(_dbName);

            var repository = new DoctorRepository(context2, mockIHttpContextAccessor.Object);

            var controller = new DoctorsController(repository, _mapper);
            var createDto = new DoctorCreateDto { FullName = "Test1", Identification = "12345678901", SpecialityId = _speciality.Id };
            var id = context2.Set<Doctor>().Last().Id;
            var result = (BadRequestObjectResult) await controller.Put(id, createDto);
            
            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.BadRequest);
            Assert.AreEqual(result.Value, $"Already exists a doctor with the identification:  {createDto.Identification}");
        }

        [Test]
        public async Task ShouldUpdateADoctor()
        {
            var initItems = new List<Doctor>
            {
                new Doctor{ FullName = "Test1", Identification = "12345678901", User = _user, Speciality = _speciality},
                new Doctor{ FullName = "Test2", Identification = "12345678902", User = _user, Speciality = _speciality},
                new Doctor{ FullName = "Test3", Identification = "12345678903", User = _user, Speciality = _speciality},
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
            var repository = new DoctorRepository(context2, httpcontext);

            var controller = new DoctorsController(repository, _mapper);
            var createDto = new DoctorCreateDto { FullName = "Test213", Identification = "12345678904", SpecialityId = _speciality.Id };
            var id = context2.Set<Doctor>().Last().Id;
            var result = (NoContentResult)await controller.Put(id, createDto);

            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.NoContent);
            var context3 = BuildContext(_dbName);
            var value = context3.Set<Doctor>().FirstOrDefault(p => p.Id == id);
           
            Assert.IsNotNull(value);
            Assert.AreEqual(value.FullName, createDto.FullName);
            Assert.AreEqual(value.Identification, createDto.Identification);
        }


        [Test]
        public async Task ShouldNotActiveOrDisactiveADoctorBecauseIdNotFound()
        {
            var httpcontext = new HttpContextAccessor();
            httpcontext.HttpContext = BuildHttpContext();
            var repository = new DoctorRepository(_context, httpcontext);

            var controller = new DoctorsController(repository, _mapper);
            controller.ControllerContext = BuildControllerContext();

            var id = Guid.NewGuid().ToString();
            var result = (NotFoundObjectResult)await controller.ActiveOrDisactive(id);

            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.NotFound);
            Assert.AreEqual(result.Value, $"Could not get a doctor with id: {id}");
        }

        [Test]
        public async Task ShouldActiveADoctor()
        {
            var entity = new Doctor 
            { 
                FullName = "Test1", 
                Identification = "12345678901", 
                User = _user, 
                Speciality = _speciality 
            };
            entity.Create(defaultUserEmail);
            entity.ActiveOrDisable(defaultUserEmail);
            _context.Set<Doctor>().Add(entity);
            _context.SaveChanges();

            var context2 = BuildContext(_dbName);
            var httpcontext = new HttpContextAccessor();
            httpcontext.HttpContext = BuildHttpContext();

            Assert.IsFalse(entity.Active);
            var repository = new DoctorRepository(context2, httpcontext);

            var controller = new DoctorsController(repository, _mapper);
            controller.ControllerContext = BuildControllerContext();
            var result = (NoContentResult)await controller.ActiveOrDisactive(entity.Id);

            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.NoContent);
            var context3 = BuildContext(_dbName);
            var dbEntity = context3.Set<Doctor>().First();
            Assert.IsTrue(dbEntity.Active);
        }

        [Test]
        public async Task ShouldDisactiveADoctor()
        {
            var entity = new Doctor
            {
                FullName = "Test1",
                Identification = "12345678901",
                User = _user,
                Speciality = _speciality
            };
            entity.Create(defaultUserEmail);
            _context.Set<Doctor>().Add(entity);
            _context.SaveChanges();

            var context2 = BuildContext(_dbName);
            var httpcontext = new HttpContextAccessor();
            httpcontext.HttpContext = BuildHttpContext();
            var repository = new DoctorRepository(context2, httpcontext);

            var controller = new DoctorsController(repository, _mapper);
            controller.ControllerContext = BuildControllerContext();
            var result = (NoContentResult)await controller.ActiveOrDisactive(entity.Id);

            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.NoContent);
            var context3 = BuildContext(_dbName);
            var dbEntity = context3.Set<Doctor>().First();
            Assert.IsFalse(dbEntity.Active);
        }

        [Test]
        public async Task ShouldNotDeleteADoctorBecauseIdNotFound()
        {
            var httpcontext = new HttpContextAccessor();
            httpcontext.HttpContext = BuildHttpContext();
            var repository = new DoctorRepository(_context, httpcontext);

            var controller = new DoctorsController(repository, _mapper);
            controller.ControllerContext = BuildControllerContext();

            var id = Guid.NewGuid().ToString();
            var result = (NotFoundObjectResult)await controller.Delete(id);

            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.NotFound);
            Assert.AreEqual(result.Value, $"Could not get a doctor with id: {id}");
        }

        [Test]
        public async Task ShouldDeleteADoctor()
        {
            var entity = new Doctor
            {
                FullName = "Test1",
                Identification = "12345678901",
                User = _user,
                Speciality = _speciality
            };
            entity.Create(defaultUserEmail);
            _context.Set<Doctor>().Add(entity);
            _context.SaveChanges();

            var context2 = BuildContext(_dbName);
            var httpcontext = new HttpContextAccessor();
            httpcontext.HttpContext = BuildHttpContext();
            var repository = new DoctorRepository(context2, httpcontext);

            var controller = new DoctorsController(repository, _mapper);
            controller.ControllerContext = BuildControllerContext();

            var result = (NoContentResult)await controller.Delete(entity.Id);

            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.NoContent);
            var context3 = BuildContext(_dbName);
            var dbEntity = context3.Set<Doctor>().FirstOrDefault();
            Assert.IsNull(dbEntity);
        }

        private DependentValue AddDependentValues()
        {
            var user = new User
            {
                Email = defaultUserEmail,
                ExternalId = Guid.NewGuid().ToString(),
                FullName = "Test User"
            };
            var speciality = new Speciality { Name = "Test 1" };
            user.Create(defaultUserEmail);
            speciality.Create(defaultUserEmail);
            _context.Add(user);
            _context.Add(speciality);
            _context.SaveChanges();
            return new DependentValue(user, speciality);
        }
    }
    internal record DependentValue(User User, Speciality Speciality);
}
