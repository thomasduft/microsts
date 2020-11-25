import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { ScopeDashboardComponent } from './scope-dashboard.component';

describe('ScopeDashboardComponent', () => {
  let component: ScopeDashboardComponent;
  let fixture: ComponentFixture<ScopeDashboardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ScopeDashboardComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScopeDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
