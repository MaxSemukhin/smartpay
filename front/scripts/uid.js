function uid(){
	let uid_text = document.getElementById("uid_textarea");
	sessionStorage.setItem('uid', uid_text.value);

	location.href = 'main.html';
}