import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl, ValidatorFn, FormArray } from '@angular/forms';

import 'rxjs/add/operator/debounceTime';

import { IPalindrome } from '../services/Palindrome';
import { PalindromeService } from '../services/palindrome.service';


function ratingRange(min: number, max: number): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
        if (c.value !== undefined && (isNaN(c.value) || c.value < min || c.value > max)) {
            return { 'range': true };
        };
        return null;
    };
}

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {
    ispalindrome: boolean = false;
    palindromes: string[];
    palindromeForm: FormGroup;
    palindrome: IPalindrome;
    errorMessage: string;

    private validationMessages = {
        required: 'Please enter your email address.',
        pattern: 'Please enter a valid email address.'
    };

    constructor(private fb: FormBuilder, private palindromeService: PalindromeService) {
    }

    ngOnInit(): void {
        this.get();

        this.palindromeForm = this.fb.group({
            stringToSave: ['', [Validators.required]]
        });

    }

    populateTestData(): void {
        this.palindromeForm.patchValue({
            stringToSave: 'Jack'
        });
    }

    save(): void {
        console.log('Saved: ' + JSON.stringify(this.palindromeForm.value));
        if (this.palindromeForm.dirty && this.palindromeForm.valid) {
            let p = Object.assign({}, this.palindrome, this.palindromeForm.value);

            this.palindromeService.savePalindrome(p)
                .subscribe(
                (data: any) => { this.onSaveComplete(data); this.get(); },
                (error: any) => this.errorMessage = <any>error
                );
        } else if (!this.palindromeForm.dirty) {
            this.onSaveComplete(null);
        }
    }

    onSaveComplete(data: any): void {
        if (data) {
            if (data.isPalindrome) {
                this.ispalindrome = data.isPalindrome;
                this.errorMessage = 'You have got a palindrome there';
            }
            else {
                this.ispalindrome = false;
                this.errorMessage = 'Nah! thats not a palindrome..';
            }
            this.get();
        }
    }

    onGetComplete(data: any): void {
        if (data) {
            if (data.palindromes)
                this.palindromes = data.palindromes;
        }
    }

    get(): void {
        this.palindromeService.get()
            .subscribe(
            (data: any) => this.onGetComplete(data),
            (error: any) => this.errorMessage = <any>error
            );
    }

}
