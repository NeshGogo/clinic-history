import { TestBed } from '@angular/core/testing';

import { SpecialityService } from './speciality.service';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { Speciality, SpecialityCreateDto } from '../models/speciality';
import { environment } from 'src/environments/environment';

describe('SpecialityService', () => {
  let service: SpecialityService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });
    service = TestBed.inject(SpecialityService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should add a speciality', (done) => {
    const createDto: SpecialityCreateDto = {
      name: 'Test Test',
    };
    const entity: Speciality = {
      name: 'Test Test',
      id: '123',
      recordCreated: new Date(),
      active: true,
    };
    service.add(createDto).subscribe((p) => {
      expect(p).toEqual(entity);
      done();
    });
    const req = httpMock.expectOne(
      `${environment.doctorServiceApi}/specialities`
    );
    expect(req.request.method).toBe('POST');
    req.flush(entity);
  });

  it('should get all speciality', (done) => {
    const entities: Speciality[] = [
      {
        name: 'Test Test1',
        id: '122313',
        recordCreated: new Date(),
        active: true,
      },
      {
        name: 'Test Test2',
        id: '123213',
        recordCreated: new Date(),
        active: true,
      },
      {
        name: 'Test Test3',
        id: '12213',
        recordCreated: new Date(),
        active: true,
      },
    ];
    service.getAll().subscribe((p) => {
      expect(p).toEqual(entities);
      done();
    });
    const req = httpMock.expectOne(
      `${environment.doctorServiceApi}/specialities`
    );
    expect(req.request.method).toBe('GET');
    req.flush(entities);
  });

  it('should update a speciality', (done) => {
    const createDto: SpecialityCreateDto = {
      name: 'Test Test',
    };
    const entities: Speciality = {
      name: createDto.name,
      id: '122313',
      recordCreated: new Date(),
      active: true,
    };
    service.update('122313', createDto).subscribe((p) => {
      expect(p).toEqual(entities);
      done();
    });
    const req = httpMock.expectOne(
      `${environment.doctorServiceApi}/specialities/${entities.id}`
    );
    expect(req.request.method).toBe('PUT');
    req.flush(entities);
  });
});
