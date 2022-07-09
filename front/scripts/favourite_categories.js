let categories = [];

function food_choice(){
	let elem = document.getElementById("food_span");
	try{
		if(!elem && ($('span').length) < 3){
			categories.push(0);
			$( "<span id='food_span' class='span'></span>" ).insertAfter( ".food" );
		}
		else{
			let new_categories = categories.filter(function(f) { return f !== 0 })
			categories = [];
			categories = new_categories;
			elem.remove();
		}
	}catch(e) {
		alert("Вы можете выбрать только 3 категории");
		console.log(e);
	}
}

function electronics_choice(){
	let elem = document.getElementById("electronics_span");
	try{
		if(!elem && ($('span').length) < 3){
			categories.push(1);
			$( "<span id='electronics_span' class='span'></span>" ).insertAfter( ".electronics" );
		}
		else{
			let new_categories = categories.filter(function(f) { return f !== 1 })
			categories = [];
			categories = new_categories;
			elem.remove();
		}
	}catch(e) {
		alert("Вы можете выбрать только 3 категории");
		console.log(e);
	}
}

function auto_choice(){
	let elem = document.getElementById("auto_span");
	try{
		if(!elem && ($('span').length) < 3){
			categories.push(2);
			$( "<span id='auto_span' class='span'></span>" ).insertAfter( ".auto" );
		}
		else{
			let new_categories = categories.filter(function(f) { return f !== 2 })
			categories = [];
			categories = new_categories;
			elem.remove();
		}
	}catch(e) {
		alert("Вы можете выбрать только 3 категории");
		console.log(e);
	}
}

function medicine_choice(){
	let elem = document.getElementById("medicine_span");
	try{
		if(!elem && ($('span').length) < 3){
			categories.push(3);
			$( "<span id='medicine_span' class='span'></span>" ).insertAfter( ".medicine" );
		}
		else{
			let new_categories = categories.filter(function(f) { return f !== 3 })
			categories = [];
			categories = new_categories;
			elem.remove();
		}
	}catch(e) {
		alert("Вы можете выбрать только 3 категории");
		console.log(e);
	}
}

function gas_choice(){
	let elem = document.getElementById("gas_span");
	try{
		if(!elem && ($('span').length) < 3){
			categories.push(4);
			$( "<span id='gas_span' class='span'></span>" ).insertAfter( ".gas" );
		}
		else{
			let new_categories = categories.filter(function(f) { return f !== 4 })
			categories = [];
			categories = new_categories;
			elem.remove();
		}
	}catch(e) {
		alert("Вы можете выбрать только 3 категории");
		console.log(e);
	}
}

function nextPage(){
	if(($('span').length) == 3){
		sessionStorage.setItem('category1', categories[0]);
		sessionStorage.setItem('category2', categories[1]);
		sessionStorage.setItem('category3', categories[2]);

		//api запрос на отправку категорий на серв

		location.href = 'favourite_shops.html';
	}

	else {
		alert("Нужно выбрать 3 категории");
	}
}
