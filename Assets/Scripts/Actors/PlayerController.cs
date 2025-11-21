using UnityEngine;

namespace Actors
{
    public class PlayerController : MonoBehaviour
    {
        public Hand Hand { get; private set; }
        private Models.Expression.Expression expression;
        public int Credits { get; set; }

        private bool isSquareRootPending;
        private Models.Cards.SpecialCard pendingSquareRootCard;

        private void Awake()
        {
            Initialize();

            ActorManager.SetPlayer(this);
        }

        private void Initialize()
        {
            Hand = new Hand();
            expression = new Models.Expression.Expression();

            isSquareRootPending = false;
            pendingSquareRootCard = null;
        }

        public void HandleCardClicked(Models.Cards.Card card)
        {
            if (card is Models.Cards.NumberCard numberCard)
            {
                HandleNumberCardClicked(numberCard);
            }
            else if (card is Models.Cards.OperatorCard operatorCard)
            {
                HandleOperatorCardClicked(operatorCard);
            }
            else if (card is Models.Cards.SpecialCard specialCard)
            {
                HandleSpecialCardClicked(specialCard);
            }
        }

        public void AddCard(Models.Cards.Card card)
        {
            if (Hand == null)
            {
                return;
            }

            Hand.AddCard(card);
        }

        public Models.Expression.Expression GetExpression()
        {
            return expression.Clone();
        }

        public void ResetHand()
        {
            Hand.Clear();
        }

        public void Prepare()
        {
            Hand.ResetCardsUsage();

            isSquareRootPending = false;
            pendingSquareRootCard = null;

            expression.Clear();
        }

        public bool IsAllNumberCardsUsed()
        {
            if (Hand == null)
            {
                return false;
            }

            foreach (var card in Hand.NumberCards)
            {
                if (!card.IsUsed)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsAllSpecialCardsUsed()
        {
            if (Hand == null)
            {
                return false;
            }

            foreach (var specialCard in Hand.SpecialCards)
            {
                if (!specialCard.IsUsed)
                {

                    return false;
                }
            }

            return true;
        }

        private void HandleNumberCardClicked(Models.Cards.NumberCard numberCard)
        {
            if (!Hand.NumberCards.Contains(numberCard))
            {
                return;
            }

            if (!expression.ExpectingNumber())
            {
                Events.UIEvents.InvokeStatusTextUpdated("지금은 연산자 카드를 눌러주세요");
                return;
            }

            bool applySquareRoot = isSquareRootPending && pendingSquareRootCard != null;

            expression.AddNumber(numberCard.Value, applySquareRoot);
            numberCard.MarkAsUsed();
            Events.CardEvents.InvokeCardUsed(numberCard);

            if (applySquareRoot)
            {
                pendingSquareRootCard.MarkAsUsed();
                Events.CardEvents.InvokeCardUsed(pendingSquareRootCard);

                isSquareRootPending = false;
                pendingSquareRootCard = null;

                Events.UIEvents.InvokeStatusTextUpdated("√ 카드가 적용되었습니다\n 연산자를 선택하세요");
            }

            Events.UIEvents.InvokeExpressionUpdated(expression.ToString());

            Events.UIEvents.InvokeStatusTextUpdated("연산자 카드를 눌러주세요");
        }

        private void HandleOperatorCardClicked(Models.Cards.OperatorCard operatorCard)
        {
            if (!Hand.OperatorCards.Contains(operatorCard))
            {
                return;
            }

            if (expression.ExpectingNumber() || expression.IsEmpty())
            {
                Events.UIEvents.InvokeStatusTextUpdated("지금은 숫자 카드를 눌러주세요");
                return;
            }

            if (IsAllNumberCardsUsed())
            {
                Events.UIEvents.InvokeStatusTextUpdated("남은 숫자가 없어 연산자를 더 선택할 수 없습니다");
                return;
            }

            expression.AddOperator(operatorCard.Operator);
            operatorCard.MarkAsUsed();
            Events.CardEvents.InvokeCardUsed(operatorCard);

            Events.UIEvents.InvokeExpressionUpdated(expression.ToString());

            Events.UIEvents.InvokeStatusTextUpdated("숫자 카드를 눌러주세요");
        }

        private void HandleSpecialCardClicked(Models.Cards.SpecialCard specialCard)
        {
            if (!Hand.SpecialCards.Contains(specialCard))
            {
                return;
            }

            if (specialCard.Type == Algorithm.Operator.OperatorType.Multiply)
            {
                HandleMultiplyCardClicked(specialCard);
                return;
            }

            if (specialCard.Type == Algorithm.Operator.OperatorType.SquareRoot)
            {
                HandleSquareRootCardClicked(specialCard);
                return;
            }
        }

        private void HandleMultiplyCardClicked(Models.Cards.SpecialCard multiplyCard)
        {
            if (expression.IsEmpty() || expression.ExpectingNumber())
            {
                Events.UIEvents.InvokeStatusTextUpdated("숫자를 배치한 후에 × 카드를 눌러주세요");
                return;
            }

            if (IsAllNumberCardsUsed())
            {
                Events.UIEvents.InvokeStatusTextUpdated("남은 숫자가 없어 × 카드를 사용할 수 없습니다");
                return;
            }

            expression.AddOperator(new Algorithm.Operator(Algorithm.Operator.OperatorType.Multiply));
            multiplyCard.MarkAsUsed();
            Events.CardEvents.InvokeCardUsed(multiplyCard);

            Events.UIEvents.InvokeExpressionUpdated(expression.ToString());
            
            Events.UIEvents.InvokeStatusTextUpdated("숫자 카드를 눌러주세요");
        }

        private void HandleSquareRootCardClicked(Models.Cards.SpecialCard squareRootCard)
        {
            if (isSquareRootPending)
            {
                Events.UIEvents.InvokeStatusTextUpdated("이미 준비된 √ 카드가 있습니다\n 숫자를 선택하세요");
                return;
            }

            if (!expression.ExpectingNumber())
            {
                Events.UIEvents.InvokeStatusTextUpdated("숫자를 넣을 차례에 √ 카드를 눌러주세요");
                return;
            }

            if (IsAllNumberCardsUsed())
            {
                Events.UIEvents.InvokeStatusTextUpdated("남은 숫자 카드가 없어 √ 카드를 사용할 수 없습니다");
                return;
            }

            isSquareRootPending = true;
            pendingSquareRootCard = squareRootCard;

            Events.UIEvents.InvokeStatusTextUpdated("다음에 선택하는 숫자에 √가 적용됩니다\n 숫자를 골라주세요");
        }

    }
}
