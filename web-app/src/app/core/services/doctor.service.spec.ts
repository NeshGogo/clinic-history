import { TestBed } from '@angular/core/testing';

import { DoctorService } from './doctor.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Doctor, DoctorCreateDto } from '../models/doctor';
import { environment } from 'src/environments/environment';

describe('DoctorService', () => {
  let service: DoctorService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });
    service = TestBed.inject(DoctorService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should add a doctor', (done) => {
    const createDto: DoctorCreateDto = {
      fullName: 'Test Test',
      identification: '12345678901',
      specialityId: '2112312'
    };
    const entity: Doctor = {
      fullName: 'Test Test',
      id: '123',
      recordCreated: new Date(),
      active: true,
      identification: '12345678901',
      specialityId: '2112312',
    };
    service.add(createDto).subscribe((p) => {
      expect(p).toEqual(entity);
      done();
    });
    const req = httpMock.expectOne(
      `${environment.doctorServiceApi}/doctors`
    );
    expect(req.request.method).toBe('POST');
    req.flush(entity);
  });

  it('should get public doctors', (done) => {
    const entities: Doctor[] = [
      {
        fullName: 'Test Test1',
        id: '122313',
        recordCreated: new Date(),
        active: false,
        identification: '12345678901',
        specialityId: '2112312',
      },
      {
        fullName: 'Test Test2',
        id: '123213',
        recordCreated: new Date(),
        active: true,
        identification: '12345678901',
        specialityId: '2112312',
      },
      {
        fullName: 'Test Test3',
        id: '12213',
        recordCreated: new Date(),
        active: true,
        identification: '12345678901',
        specialityId: '2112312',
      },
    ];
    service.getPublic().subscribe((p) => {
      expect(p).toEqual(entities.filter(p => p.active));
      done();
    });
    const req = httpMock.expectOne(
      `${environment.doctorServiceApi}/doctors/active`
    );
    expect(req.request.method).toBe('GET');
    req.flush(entities.filter(p => p.active));
  });

  it('should get all doctors', (done) => {
    const entities: Doctor[] = [
      {
        fullName: 'Test Test1',
        id: '122313',
        recordCreated: new Date(),
        active: true,
        identification: '12345678901',
        specialityId: '2112312',
      },
      {
        fullName: 'Test Test2',
        id: '123213',
        recordCreated: new Date(),
        active: true,
        identification: '12345678901',
        specialityId: '2112312',
      },
      {
        fullName: 'Test Test3',
        id: '12213',
        recordCreated: new Date(),
        active: true,
        identification: '12345678901',
        specialityId: '2112312',
      },
    ];
    service.getAll().subscribe((p) => {
      expect(p).toEqual(entities);
      done();
    });
    const req = httpMock.expectOne(
      `${environment.doctorServiceApi}/doctors`
    );
    expect(req.request.method).toBe('GET');
    req.flush(entities);
  });

  it('should update a doctor', (done) => {
    const createDto: DoctorCreateDto = {
      fullName: 'Test Test',
      identification: '12345678901',
      specialityId: '2112312'
    };
    const entities: Doctor = {
      fullName: createDto.fullName,
      id: '122313',
      recordCreated: new Date(),
      active: true,
      identification: '12345678901',
      specialityId: '2112312'
    };
    service.update('122313', createDto).subscribe((p) => {
      expect(p).toEqual(entities);
      done();
    });
    const req = httpMock.expectOne(
      `${environment.doctorServiceApi}/doctors/${entities.id}`
    );
    expect(req.request.method).toBe('PUT');
    req.flush(entities);
  });

  it('should active or disactive a doctor', (done) => {
    service.ActiveOrDisactive('122313').subscribe(() => {
      done();
    });
    const req = httpMock.expectOne(
      `${environment.doctorServiceApi}/doctors/ActiveOrDisactive/122313`
    );
    expect(req.request.method).toBe('PUT');
    req.flush({});
  });
});
