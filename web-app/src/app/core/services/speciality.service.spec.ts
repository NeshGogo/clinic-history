import { TestBed } from '@angular/core/testing';

import { SpecialityService } from './speciality.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

describe('SpecialityService', () => {
  let service: SpecialityService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule]
    });
    service = TestBed.inject(SpecialityService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
