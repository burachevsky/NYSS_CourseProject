using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrypterCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrypterCoreTests
{
    [TestClass()]
    public class VigenereCipherTests
    {
        /*[TestMethod()]
        public void VigenereCipherTest()
        {
            Assert.Fail();
        }*/

        [TestMethod()]
        public void DecryptTest1()
        {
            var cipher = new VigenereCipher("скорпион", Alphabets.RUSSIAN);
            var result = cipher.Decrypt("бщцфаирщри, бл ячъбиуъ щбюэсяёш гфуаа!!! ");
            Console.WriteLine(result);
            Assert.AreEqual("поздравляю, ты получил исходный текст!!! ", result);
        }

        [TestMethod()]
        public void DecryptTest2()
        {
            var input = "лол здарова";
            var key = "ну здарова";
            var res = "юыд ддпвмвт";

            var cipher = new VigenereCipher(key, Alphabets.RUSSIAN);
            var result = cipher.Decrypt(input);
            Assert.AreEqual(res, result);
        }

        [TestMethod()]
        public void EncryptTest1()
        {
            var input = "лол здарова";
            var key = "ну здарова";
            var res = "щву лдрярвн";

            var cipher = new VigenereCipher(key, Alphabets.RUSSIAN);
            var result = cipher.Encrypt(input);
            Assert.AreEqual(res, result);
        }

        [TestMethod]
        public void DecryptTest3()
        {
            var input = "бщцфаирщри, бл ячъбиуъ щбюэсяёш гфуаа!!! \r\nу ъящэячэц ъэюоык, едщ бдв саэацкшгнбяр гчеа кчфцшубп цу ьгщпя вщвсящ, эвэчрысй юяуъщнщхо шпуъликугбз чъцшья с цощъвчщ ъфмес ю лгюлэ ёъяяр! с моыящш шпмоец щаярдш цяэубфъ аьгэотызуа дщ, щръ кй юцкъщчьуац уыхэцэ ясч юбюяуяг ыовзсгюамщщ. внютвж тхыч эядкъябе цн юкъль, мэсццогл шяьфыоэьь ть эщсщжнашанэ ыюцен, уёюяыцчан мах гъъьуун шпмоыъй ч яяьпщъхэтпык яущм бпйэае! чэьюмуд, оээ скфч саьбрвчёыа эядуцйт ъ уьгфщуяяёу фси а эацэтшцэч юпапёи, ьь уъубфмч ысь хффы ужц чьяцнааущ эгъщйаъф, ч п эиттпьк ярвчг гмубзньцы! щб ьшяо шачюрэсч FirstLineSoftware ц ешчтфщацдпбр шыыь, р ыоф ячцсвкрщве бттй а ядсецсцкюкх эшашёрэсуъ якжще увюгщр в# уфн ысвчюпжзцж! чй ёюычъ бщххыибй еьюхечр п хкъмэншёцч юятщвфцшчщ с хчю ъэ ч аачсюсчыщачрняун в шъюьэжцясиьццч агфуо ацаьяычсцы .Net, чэбф ыуюбпьщо с чыдпяхбцйг щктрж!";
            var key = "скорпион";
            var res = "поздравляю, ты получил исходный текст!!! \r\nв принципе понять, что тут используется шифр виженера не особо трудно, основная подсказка заключается именно в наличии ключа у этого шифра! в данной задаче особый интерес составляет то, как вы реализуете именно сам процесс расшифровки. теперь дело осталось за малым, доделать программу до логического конца, выполнить все условия задания и опубликовать свою работу! молодец, это были достаточно трудные и интересные два с половиной месяца, но впереди нас ждет еще множество открытий, и я надеюсь общих свершений! от лица компании FirstLineSoftware и университета итмо, я рад поздравить тебя с официальным окончанием наших курсов с# для начинающих! мы хотим пожелать успехов в дальнейшем погружении в мир ит и программирования с использованием стека технологий .Net, море терпения и интересных задач!";

            var cipher = new VigenereCipher(key, Alphabets.RUSSIAN);
            var result = cipher.Decrypt(input);
            Assert.AreEqual(res, result);
        }

        [TestMethod]
        public void EncryptTest2()
        {
            try
            {
                var input = "ATTACKATDAWN";
                var key = "LEMONLEMONLE";
                var res = "LXFOPVEFRNHR";

                var cipher = new VigenereCipher(key, Alphabets.ENGLISH);
                var result = cipher.Encrypt(input);
                Assert.AreEqual(res, result);
            }
            catch (Exception e)
            {
                Assert.Fail(e.GetType() + " " + e.StackTrace);
            }
        }
    }
}