import { TestBed } from '@angular/core/testing';

import { ClinicRecordService } from './clinic-record.service';

describe('ClinicRecordService', () => {
  let service: ClinicRecordService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ClinicRecordService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
