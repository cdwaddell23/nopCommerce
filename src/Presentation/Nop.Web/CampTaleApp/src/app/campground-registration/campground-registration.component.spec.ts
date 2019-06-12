import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CampgroundRegistrationComponent } from './campground-registration.component';

describe('CampgroundRegistrationComponent', () => {
  let component: CampgroundRegistrationComponent;
  let fixture: ComponentFixture<CampgroundRegistrationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CampgroundRegistrationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CampgroundRegistrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
