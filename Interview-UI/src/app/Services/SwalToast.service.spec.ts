import { TestBed } from '@angular/core/testing';

import { SwalToastService } from './SwalToast.service';

describe('SwalToastService', () => {
  let service: SwalToastService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SwalToastService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
