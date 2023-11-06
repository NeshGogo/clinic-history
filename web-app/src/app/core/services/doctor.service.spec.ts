import { TestBed } from '@angular/core/testing';

import { DoctorService } from './doctor.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('DoctorService', () => {
  let service: DoctorService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });
    service = TestBed.inject(DoctorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
