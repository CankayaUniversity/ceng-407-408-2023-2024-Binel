
function isValidEmail(email) {
  // Email için geçerli bir düzenli ifade
  var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email);
}

document.addEventListener('DOMContentLoaded', function() {
  // Toggle password visibility in login modal
  document.getElementById('togglePassword').addEventListener('click', function() {
    const passwordInput = document.getElementById('password');
    const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
    passwordInput.setAttribute('type', type);
    this.querySelector('i').classList.toggle('fa-eye-slash');

      // Change button text based on password visibility
      const buttonText = document.getElementById('passwordToggleText');
      if (type === 'text') {
          buttonText.textContent = 'Şifreyi gizle';
      } else {
          buttonText.textContent = 'Şifreyi göster';
      }
  });

  // Toggle password visibility in register modal
  document.getElementById('toggleRegisterPassword').addEventListener('click', function() {
    const passwordInput = document.getElementById('registerPassword');
    const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
    passwordInput.setAttribute('type', type);
    this.querySelector('i').classList.toggle('fa-eye-slash');

    const passwordInput2 = document.getElementById('passwordConfirm');
    const type2 = passwordInput2.getAttribute('type') === 'password' ? 'text' : 'password';
    passwordInput2.setAttribute('type', type2);
    this.querySelector('i').classList.toggle('fa-eye-slash');

     // Change button text based on password visibility
     const buttonText = document.getElementById('passwordToggleText2');
     if (type === 'text') {
         buttonText.textContent = 'Şifreyi gizle';
     } 
     else {
         buttonText.textContent = 'Şifreyi göster';
     }
  });

  // Form submission for login
  document.getElementById('login-form').addEventListener('submit', function(event) {
    event.preventDefault(); // Formun varsayılan davranışını engelle

    // Kullanıcı adı ve şifre alanlarını al
    var username = document.getElementById('username').value;
    var password = document.getElementById('password').value;

    // Basit bir kontrol: Kullanıcı adı ve şifre boş olmamalıdır
    if (username === '' || password === '') {
      document.getElementById('error-message1').innerText = 'Lütfen kullanıcı adı ve şifrenizi girin.';
      return;
    }

    // Burada giriş işlemi yapılabilir, bu örnekte sadece konsola mesaj basıyoruz
    console.log('Kullanıcı adı:', username);
    console.log('Şifre:', password);

    // Giriş başarılı olduğunda modalı gizle
    $('#loginModal').modal('hide');
  });

  // Giriş yap butonuna tıklandığında modalı göster
  document.getElementById('loginButton').addEventListener('click', function() {
    $('#loginModal').modal('show');
  });

  // Form submission for registration
  document.getElementById('register-form').addEventListener('submit', function(event) {
    event.preventDefault(); // Formun varsayılan davranışını engelle

    // Gerekli tüm alanları al
    var firstname = document.getElementById('firstname').value;
    var lastname = document.getElementById('lastname').value;
    var registerUsername = document.getElementById('registerUsername').value;
    var email = document.getElementById('email').value;
    var registerPassword = document.getElementById('registerPassword').value;
    var passwordConfirm = document.getElementById('passwordConfirm').value;

    // Email doğruluğunu kontrol et
    if (!isValidEmail(email)) {
      document.getElementById('error-message').innerText = 'Geçerli bir email adresi girin.';
      return;
    }

    // Şifrelerin eşleştiğini kontrol et
    if (registerPassword !== passwordConfirm) {
      document.getElementById('error-message').innerText = 'Şifreler eşleşmiyor.';
      return;
    }

    // Basit bir kontrol: Gerekli tüm alanlar dolu olmalıdır
    if (firstname === '' || lastname === '' || registerUsername === '' || email === '' || registerPassword === '' || passwordConfirm === '') {
      document.getElementById('error-message').innerText = 'Lütfen tüm alanları doldurun.';
      return;
    }

    // Burada kayıt işlemi yapılabilir, bu örnekte sadece konsola mesaj basıyoruz
    console.log('Ad:', firstname);
    console.log('Soyad:', lastname);
    console.log('Kullanıcı adı:', registerUsername);
    console.log('E-posta:', email);
    console.log('Şifre:', registerPassword);
    console.log('Şifre Onayı:', passwordConfirm);

    // Kayıt başarılı olduğunda modalı gizle
    $('#registerModal').modal('hide');
  });

  // Kayıt ol butonuna tıklandığında modalı göster
  document.getElementById('registerButton').addEventListener('click', function() {
    $('#registerModal').modal('show');
  });
});


//Bu kodlar, fare arama butonunun üzerine geldiğinde ve üzerinden çekildiğinde resminin değişmesini sağlar. search-bar-01.png ve search-bar-02.png dosyalarının gerçek yolunu belirterek bu kısmı güncelleyebilirsiniz.
function changeImage() {
  document.getElementById("search-img").src = "search-bar-02.png";
}

function restoreImage() {
  document.getElementById("search-img").src = "search-bar-01.png";
}
