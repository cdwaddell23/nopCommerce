import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CampgroundReviewComponent } from './campground-review.component';

describe('CampgroundReviewComponent', () => {
  let component: CampgroundReviewComponent;
  let fixture: ComponentFixture<CampgroundReviewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CampgroundReviewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CampgroundReviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
