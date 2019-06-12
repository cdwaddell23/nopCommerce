import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';
import { FormControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FileHolder } from 'angular2-image-upload'
import 'rxjs/Rx';
import 'hammerjs';

@Component({
  selector: 'campground-review',
  templateUrl: './campground-review.component.html',
  styleUrls: ['./campground-review.component.css'],
  encapsulation: ViewEncapsulation.Native
})
export class CampgroundReviewComponent implements OnInit {
  @Input() campgroundId: number = 0;
  API_VERSION = '/camping/api/v1/';
  isLinear = false;
  campgroundReview: CampgroundReviewModel;

  ratingFormGroup: FormGroup;
  reviewFormGroup: FormGroup;
  picturesFormGroup: FormGroup;
  ratingCtrl: FormControl;
  reviewTitleCtrl: FormControl;
  reviewBodyCtrl: FormControl;
  pictureCtrl: FormControl;

  constructor(private _formBuilder: FormBuilder, private _httpClient: HttpClient, private _http: Http) {
    this.campgroundReview = new CampgroundReviewModel;
  }

  ngOnInit() {
    this.ratingCtrl = new FormControl(null, Validators.required);
    this.reviewTitleCtrl = new FormControl(null, Validators.required);
    this.reviewBodyCtrl = new FormControl(null, Validators.required);
    this.ratingFormGroup = this._formBuilder.group({
      ratingCtrl: ['', Validators.required],
    });
    this.reviewFormGroup = this._formBuilder.group({
      reviewTitleCtrl: ['', Validators.maxLength(100)],
      reviewBodyCtrl: ['', Validators.maxLength(2000)],
    });
    this.picturesFormGroup = this._formBuilder.group({
      pictureCtrl: ['', Validators.nullValidator],
    });
  }

  getReviews(cid) {
    this._httpClient.get(this.API_VERSION + 'campground/review/find-review/' + cid.toString())
      .subscribe((response: Response) => {
        this.campgroundReview.reviewid = response['reviewid'];
        this.campgroundReview.name = response['name'];
        this.campgroundReview.rating = response['rating'];
        this.campgroundReview.title = response['title'];
        this.campgroundReview.description = response['description'];
        this.campgroundReview.pictures = response['pictures'];
        response['rating'] > 0 ? this.ratingCtrl.setValue(response['rating']) : this.ratingCtrl.setValue(null);
        response['title'].length > 0 ? this.reviewTitleCtrl.setValue(response['title']) : this.reviewTitleCtrl.setValue(null);
        response['description'].length > 0 ? this.reviewBodyCtrl.setValue(response['description']) : this.reviewBodyCtrl.setValue(null);
      });
  }

  addRevew(formData): void {
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    headers.append('Content-Type', 'multipart/form-data');
    let body = JSON.stringify({ campgroundId: this.campgroundId || 0, Rating: this.ratingCtrl.value || 0, Title: this.reviewTitleCtrl.value, ReviewText: this.reviewBodyCtrl.value });
    this._httpClient.post(this.API_VERSION + 'campground/review/add-review/', body, { headers })
      .subscribe((response: Response) => {
        this.campgroundReview.reviewid = response['reviewid'];
        this.campgroundReview.name = response['name'];
        this.campgroundReview.rating = response['rating'];
        this.campgroundReview.title = response['title'];
        this.campgroundReview.description = response['description'];
        this.campgroundReview.pictures = response['pictures'];
      })
  }

  onRemoved(file: FileHolder): void {
    console.log(file)
  }

  resetReview(): void {
    document.getElementById('addCampgroundReviews-' + this.campgroundId).style.display = "none";
  }

}

export class CampgroundReviewModel {
  reviewid: number = 0;
  name: string = '';
  rating: number = 0;
  title: string = '';
  description: string = '';
  pictures: Array<string> = null;
  success: boolean = false;
  message: string = '';
}
