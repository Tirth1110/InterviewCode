/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { FileUploadValidationCheckService } from './FileUploadValidationCheck.service';

describe('Service: FileUploadValidationCheck', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FileUploadValidationCheckService]
    });
  });

  it('should ...', inject([FileUploadValidationCheckService], (service: FileUploadValidationCheckService) => {
    expect(service).toBeTruthy();
  }));
});
