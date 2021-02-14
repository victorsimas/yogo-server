using System;
using System.Linq;
using Xunit;
using YogoServer.Requests;
using YogoServer.Responses;

namespace YogoServer.UnitTests.Methods
{
    public class TestYogoBinariesCall
    {
        public const string listEmailForTest = 
            " 1 Loja.com.br\n Psiu, psiu 😆 Esse cupom tá BOMBANDO 💰💥\n\n" +
            " 2 Loja.com.br\n Desbloqueie um CUPOM EXTRA confidencial 👀🔐\n\n" +
            " 3 LugarGelido pra você\n Você deixou essa oferta no seu carrinho!\n\n" +
            " 4 CasaQuente com você\n Seu carrinho te espera!\n\n" +
            " 5 Loja.com.br\n Chegou a hora de mudar! 🤗 Móveis com até 15% OFF, vem!\n\n";

        public const string showEmailForTest =
            "---\nFrom  : LugarGelido \nTitle : Código de segurança LugarGelido\nDate  : 2021-02-12 07:53\n---"+
            "\n*Olá Janio,*\n\n*QUEREMOS OFERECER UM SERVIÇO*\n\n*CADA VEZ MAIS DESCOMPLICADO E SEGURO PARA VOCÊ*\n\nEste é o código de segurança da LugarGelido, utilize agora\n"+
            "\nmesmo para concluir sua verificação de segurança em nosso site.\n\n*213695*\n\n*Você não reconhece isso? Entre em contato conosco.*\n"+
            "\nAjudaremos a manter a sua conta em segurança. Do contrário, você não precisa fazer nada.\n\n*Dúvidas?*\n";

        [Fact]
        public async void EmailsDefineAsync()
        {
            var inboxRequest = new InboxListRequest() { Amount = 5 };

            Emails emails = await inboxRequest.DefineAsync(listEmailForTest);

            Assert.Equal(inboxRequest.Amount, emails.Inbox.Count);
            Assert.Contains(
                listEmailForTest.Split(
                    Environment.NewLine, 
                    StringSplitOptions.RemoveEmptyEntries).First().Replace(" ", string.Empty), 
                emails.Inbox.First().Replace(" ", string.Empty));
        }

        [Fact]
        public async void EmailDefineAsync()
        {
            var inboxRequest = new InboxMailRequest() { Index = 5, User = "Janio"};

            Email email = await inboxRequest.DefineAsync(showEmailForTest);

            Assert.NotNull(email.HeadEmail);
            Assert.True(email.Body.Count > default(int));
            Assert.Contains(inboxRequest.User, email.Body.First());
        }
    }
}