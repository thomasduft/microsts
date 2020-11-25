import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { RoleDashboardComponent } from './role-dashboard.component';

describe('RoleDashboardComponent', () => {
  let component: RoleDashboardComponent;
  let fixture: ComponentFixture<RoleDashboardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ RoleDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RoleDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
