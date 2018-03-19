import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/of';

import { IPalindrome } from './Palindrome';

@Injectable()
export class PalindromeService {
    private baseUrl = 'api/Palindrome/';

    constructor(private http: Http) { }

    savePalindrome( palindrome: string): Observable<IPalindrome> {
        let headers = new Headers({ 'Content-Type': 'application/json' });

        let options = new RequestOptions({ headers: headers });

        return this.createPalindrome(options, palindrome);
    }

    get(): Observable<IPalindrome> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        return this.getPalindrome(options);
    } 

    private createPalindrome(options: RequestOptions, palindrome: string): Observable<IPalindrome> {
        return this.http.post(this.baseUrl + 'CheckPalindrome', JSON.stringify(palindrome), options)
            .map(this.extractData)
            .do(data => console.log('Is given string a palindrome ? : ' + JSON.stringify(data)))
            .catch(this.handleError);
    }

    public getPalindrome(options: RequestOptions): Observable<IPalindrome> {
        return this.http.get('http://localhost:57555/api/Palindrome/', options)
            .map(this.extractData)
            .do(data => console.log('Is given string a palindrome ? : ' + JSON.stringify(data)))
            .catch(this.handleError);
    }

    private extractData(response: Response) {
        let body = response.json();
        return body.data || {};
    }

    private handleError(error: Response): Observable<any> {
        // in a real world app, we may send the server to some remote logging infrastructure
        // instead of just logging it to the console
        console.error(error);
        return Observable.throw(error.json().error || 'Server error');
    }

}
