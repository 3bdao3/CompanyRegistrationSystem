import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidateOtp } from './validate-otp';

describe('ValidateOtp', () => {
  let component: ValidateOtp;
  let fixture: ComponentFixture<ValidateOtp>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ValidateOtp]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ValidateOtp);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
