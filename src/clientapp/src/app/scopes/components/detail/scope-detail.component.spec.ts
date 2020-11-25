import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { ScopeDetailComponent } from './scope-detail.component';

describe('ScopeDetailComponent', () => {
  let component: ScopeDetailComponent;
  let fixture: ComponentFixture<ScopeDetailComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ScopeDetailComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScopeDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
